USE master;
GO

USE VolleyManagement;
GO

/*************************************************
  Inserting data to database
*************************************************/

INSERT INTO dbo.Tournaments(
[Name],
[Scheme],
[Season],
[Description],
[RegulationsLink]
)
VALUES
('Первый чемпионат любительской лиги', '1', '2012/2013', 'Любительская лига',''),
('Второй чемпионат любительской лиги', '1', '2013/2014', 'Любительская лига','Volley.dp.ua'),
('Третий чемпионат любительской лиги', '2', '2014/2015', 'Любительская лига','123123'),
('Четвертый чемпионат любительской лиги', '3', '2014/2015', 'Любительская лига','Volleyball.ua'),
('Пятый чемпионат любительской лиги', '1', '2014/2015', 'Любительская лига','');
GO

INSERT INTO dbo.Users(
  [UserName],
  [Password],
  [FullName],
  [CellPhone],
  [Email]
) VALUES 
  ('admin', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Иванов Иван Иванович','(123) 456-78-91','volleyms@gmail.com'),
  ('captain',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Сидоров Сидор Сидорович','(123) 456-78-91','captain@volley.com'),
  ('userok',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Петров Пётр Петрович','(123) 456-78-91','user@user.ru'),
  ('sutuga', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Антонов Антон Антонович','(123) 456-78-91','sutuga@sutuga.ru'),
  ('glushko',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Зима Антон Антонович','(123) 456-78-91','glushko@glushko.ru'),
  ('pavel',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Снежкин Олег Евгеньевич','(123) 456-78-91','pavel@user.ru'),
  ('ivanchenko',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Ложкин Александр Александрович','(123) 456-78-91','ivanchenko@captain.ru'),
  ('pilipenko',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Вилкин Станислав Юрьевич','(123) 456-78-91','pilipenko@user.ru'),
  ('leta', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Снегопадов Юрий','(123) 456-78-91','leta@gmail.com'),
  ('vasya',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Метельский Андрей','(123) 456-78-91','vasya@vasya.ru'),
  ('reznik',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Дождевой Сергей','(123) 456-78-91','reznik@user.ru'),
  ('cap',	'32274BBB35FB029FB354C4C9556A6DA12CC223415E414E978CE287B6EAEB0977',	'','(123) 456-78-91','sms1@sms.com')
GO