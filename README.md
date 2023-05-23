# Решение заданий

<!-- Я сделал одно решение в нем два проекта

### Задание 1
Я сделал Web-Api, проект называется **TaskOne**. Запросы к базе данных осуществляется с помощью **Entity Framework** и **Dapper**. При запуске проекта отображается html страница с запросами к БД.

Список запросов:

1. Сотрудника с максимальной заработной платой.
```
SELECT TOP (1) E.Id EmpleyeeId, E.Name EmployeeName, E.Salary FROM Employee E ORDER BY E.Salary DESC
```
2. Вывести одно число: максимальную длину цепочки руководителей по таблице сотрудников (вычислить глубину дерева).
```
WITH Recursives AS (SELECT E.Id, E.ChiefId, 0 CountStep FROM Employee E 
WHERE ChiefId IS NULL
UNION ALL
SELECT T.Id, T.ChiefId, R.CountStep+1 FROM Employee T 
INNER JOIN Recursives R ON T.ChiefId = R.Id)
SELECT Max(CountStep) MaxStep FROM Recursives
```
3. Отдел, с максимальной суммарной зарплатой сотрудников.
```
SELECT TOP (1) D.Id DepartmentId, D.Name DepartmentName, SUM(E.Salary) Sum FROM Department D 
LEFT JOIN Employee E ON E.DepartmentId = D.Id 
GROUP BY D.Id, D.Name 
ORDER BY SUM(E.Salary) DESC
```
4. Сотрудника, чье имя начинается на «Р» и заканчивается на «н».
```
SELECT TOP (1) E.Id EmpleyeeId, E.Name EmployeeName FROM Employee EWHERE E.Name LIKE 'Р%н'
```

### Задание 2
Проект называется **TaskTwo** реализовал в виде консольного приложения.
-->
