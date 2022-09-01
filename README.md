# 喵屋星球

一個提供保姆、領養、協尋貓的網站

https://meowplanet.lol/Adoption

在此專案獨立負責領養的功能

後端框架使用ASP.NET Core MVC、MSSQL、Entity Framework。

前端技術Bootstrap、JQUERY、AJAX

### `領養首頁`  

前端有設計RWD響應式網頁，使用者可以在不同大小、解析度的裝置螢幕下使用。

![RWD](https://user-images.githubusercontent.com/83138682/185895130-4d01ed6e-53fe-497c-bb76-7eb37c20d685.gif)

### `認養 / PASS / 篩選`

使用Ajax取得後端資料讓使用者操作時可以局部更新畫面不會一直閃現。

後端使用了LINQ根據資料庫資料去篩選出可領養且非走失的貓貓並隨機產生一筆亂數資料回傳至前端渲染畫面。

PASS的時候是用CSS的Animation達到卡片翻轉的效果。
照片輪播牆是使用slick套件。

![動畫](https://user-images.githubusercontent.com/83138682/185892880-f2bae611-195a-4aeb-8e87-a46237427d5c.gif)

### `查看領養資訊 / 同意 / 拒絕`

![PI](https://user-images.githubusercontent.com/83138682/185896726-45cf03e1-d002-4043-af79-10b797fac747.gif)

為了使用者互動體驗，我在認養、同意、拒絕的時候有使用sweetalert2做彈出提式窗。
