using System.Linq;
using Moq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.Queries.Common;
using VolleyManagement.Data.Queries.Team;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Services;
using Xunit;
using Player = VolleyManagement.Domain.PlayersAggregate.Player;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class CreatePlayerSteps
    {
        private readonly Player _player;
        private readonly IPlayerService _playerService;

        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<IQueryable<Player>, GetAllCriteria>> _getAllPlayersQueryMock;
        private Mock<ITeamRepository> _teamRepositoryMock;
        private Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock;
        private Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        public CreatePlayerSteps()
        {
            TestInit();

            _player = new Player();

            _playerService = BuildSUT();
        }

        public void TestInit()
        {
            _authServiceMock = new Mock<IAuthorizationService>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getAllPlayersQueryMock = new Mock<IQuery<IQueryable<Player>, GetAllCriteria>>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _getTeamByIdQueryMock = new Mock<IQuery<Team, FindByIdCriteria>>();
            _getTeamByCaptainQueryMock = new Mock<IQuery<Team, FindByCaptainIdCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _playerRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        private PlayerService BuildSUT()
        {
            return new PlayerService(
                _playerRepositoryMock.Object,
                _getTeamByIdQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _getAllPlayersQueryMock.Object,
                _getTeamByCaptainQueryMock.Object,
                _authServiceMock.Object);
        }

        [Given(@"first name is (.*)")]
        public void GivenFirstNameIs(string firstName)
        {
            _player.FirstName = firstName;
        }

        [Given(@"last name is (.*)")]
        public void GivenLastNameIs(string lastName)
        {
            _player.LastName = lastName;
        }

        [When(@"I execute CreatePlayer")]
        public void WhenIExecuteCreatePlayer()
        {
            _playerService.Create(_player);
        }

        [Then(@"new player should be succesfully added")]
        public void ThenNewPlayerShouldBeSuccesfullyAdded()
        {
            _playerRepositoryMock.Verify(pr => pr.Add(It.IsAny<Player>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once());
        }

        [Then(@"new player gets new Id")]
        public void ThenNewPlayerGetsNewId()
        {
            Assert.NotEqual(default(int), _player.Id);
        }
    }
}
