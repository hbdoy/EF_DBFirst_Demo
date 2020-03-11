# EF_DBFirst_Demo
Test Entity Framework 6 on .Net MVC with DB First

## How to Use
1. ``git clone https://github.com/hbdoy/EF_DBFirst_Demo.git``
2. Restore Nuget Package
3. If you want to use some examples first => DemoLibraryMVC/ConsoleApp1

## About
It's a verrrrrrry small project to practice using ORM Framework

In simple terms, It has three ways to generate Model
- Code First
- DB First
- Model First

And this project uses the second method, DB First, to achieve basic CRUD in MVC project.

More about [Entity Framework](https://docs.microsoft.com/zh-tw/ef/ef6/)

## Usage
- .NET Framwork 4.6
- MSSQL
- Entity Framework 6 (ORM)

## Notes
初次使用可能需要學習了解的知識點:
1. 什麼是 ORM，為了什麼出現、可以用來做什麼
2. EF 中的 EDM 是什麼
3. 了解 EF Designer 中的 EDMX 結構
4. DB Table 結構的變動 & EDMX 連動關係的愛恨糾葛(Ex: 刪除 Table 欄位，EDMX 有哪些會更新、哪些不會) 
5. 一些與[關聯資料的東西](https://docs.microsoft.com/zh-tw/ef/ef6/querying/related-data)，(Ex: Include()、Load()，以及 LazyLoading 是什麼)
6. Model 資料欄位的驗證與特定的屬性，無法加在 tt 檔自動產生的 Model 中(下次更新就會不見)，可以試著使用 MetaData 來處理 

## Screenshots
### Search
![](https://i.imgur.com/B2ldumB.png)

### Update
![](https://i.imgur.com/veLRk1e.png)

### Insert
![](https://i.imgur.com/fuYS6Xb.png)
