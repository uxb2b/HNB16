# HNB WebHome 專案

## 前端 JavaScript 工具 site.js

本專案於 `WebHome/wwwroot/js/site.js` 提供多項前端輔助功能，包含表單序列化、列印、下載等。

### launchDownload 用法

`launchDownload` 主要用於觸發檔案下載，會以 POST 方式將表單資料送至指定 URL。

#### 語法

```javascript
$(formSelector).launchDownload(url, params, target, loading);
```
- `url`：下載的 server 端 action 路徑
- `params`：額外傳遞的參數 (物件)
- `target`：目標 iframe 名稱 (可選)
- `loading`：是否顯示 loading 動畫 (布林值，可選)

#### 範例

```javascript
$('#myForm').launchDownload('/Download/Export', { extraParam: 'value' }, null, true);
```

#### 注意事項
- **Controller Action 必須標註 `[HttpPost]`**
- **參數必須標註 `[FromForm]`**

##### 範例 Controller

```csharp
[HttpPost]
public IActionResult Export([FromForm] ExportViewModel model)
{
    // 處理下載邏輯
}
```

### 其他功能
- 表單序列化：`$(form).serializeObject()`
- 檔案上傳：`uploadFile($file, postData, url, callback, errorback)`
- 列印功能：`$global.printContent($element, printDone)`

---

如需更多細節，請參考 `WebHome/wwwroot/js/site.js` 原始碼註解。