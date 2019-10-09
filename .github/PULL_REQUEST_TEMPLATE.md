### Summary

_Describe main changes going into those pull requests._

### Areas Of Interest

_Any specific places you want to bring attention of the reviewers? Some places you are uncertain in/some design decisions. (If none remove this section)_

### Checklist

- [ ] Logger logs all newly added types correctly

#### Design

- [ ] Web API layer contains as little logic as possible
- [ ] Domain objects use semantic names

#### Perfromance

- [ ] In case of any DB query was changed: attach generated SQL for review
- [ ] Number of DB roundtrips should be as low as possible

#### Unit Tests

- [ ] Tests have appropriate category set: `@unit`, `@azurecloud`, `@onpremsql`
- [ ] Story/bug tag is assigned to created/affected test: `@ab:123`
- [ ] Tests resolve `IRequestHandler<,>` instance instead of concrete implementation

#### Integration Tests

- [ ] Teardown deletes test data after each test

### New Project Checklist

- [ ] Dockerfile is updated
- [ ] Azure Pipelines updated
- [ ] Development setup guide updated
- [ ] VolleyM.API has reference added to assist debugging
- [ ] Project added into required migrations

### Azure Storage

If new table added:

- [ ] Update settings for all Environments
- [ ] Update settings for integration tests
