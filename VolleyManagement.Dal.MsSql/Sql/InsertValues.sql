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
  ('admin', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Иванов Иван Иванович','+3(050)995-70-40','volleyms@gmail.com'),
  ('captain',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Сидоров Сидор Сидорович','+3(050)995-70-40','captain@volley.com'),
  ('userok',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Петров Пётр Петрович','+3(050)995-70-40','user@user.ru'),
  ('sutuga', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Антонов Антон Антонович','+3(050)995-70-40','sutuga@sutuga.ru'),
  ('glushko',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Зима Антон Антонович','+3(050)995-70-40','glushko@glushko.ru'),
  ('pavel',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Снежкин Олег Евгеньевич','+3(050)995-70-40','pavel@user.ru'),
  ('ivanchenko',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Ложкин Александр Александрович','+3(050)995-70-40','ivanchenko@captain.ru'),
  ('pilipenko',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Вилкин Станислав Юрьевич','+3(050)995-70-40','pilipenko@user.ru'),
  ('leta', 'AC31406E5DA35EF6A170901CE1136567FDEB8E7BD8089C0F4A1E86CFB7B5A2F2','Снегопадов Юрий','+3(050)995-70-40','leta@gmail.com'),
  ('vasya',  '04547EFEF7C894F6E16717D05D7D57205933C34775242C7A61F9E75B8D9F1CCB','Метельский Андрей','+3(050)995-70-40','vasya@vasya.ru'),
  ('reznik',  '53E725D146C6B38C2E1EC88F549F3CBD9641F8119582C134406C8618479FCD4F','Дождевой Сергей','+3(050)995-70-40','reznik@user.ru'),
  ('cap_1',	'32274BBB35FB029FB354C4C9556A6DA12CC223415E414E978CE287B6EAEB0977',	'','0974019341','sms1@sms.com'),
  ('cap_2',	'418BC562CD00BA7F06CA44C0A8162F1B4C142C6D53B08282DF9280B5B51713CE','',	'0974049341','sms2@sms.com'),
  ('cap_3',	'3571759416F60909E1EB1A44F0424177AFA296E6CC08997C645064B8DDE40239','','974165898','sms3@sms.com'),
  ('cap_4',	'82FAB6B5F1F4360A1C94C782DAF2E564086986B2645BB3FC0CCB88764B000D6F','','987865231','sms4@sms.com');
GO