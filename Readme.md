# HNB WebHome �M��

## �e�� JavaScript �u�� site.js

���M�ש� `WebHome/wwwroot/js/site.js` ���Ѧh���e�ݻ��U�\��A�]�t���ǦC�ơB�C�L�B�U�����C

### launchDownload �Ϊk

`launchDownload` �D�n�Ω�Ĳ�o�ɮפU���A�|�H POST �覡�N����ưe�ܫ��w URL�C

#### �y�k

```javascript
$(formSelector).launchDownload(url, params, target, loading);
```
- `url`�G�U���� server �� action ���|
- `params`�G�B�~�ǻ����Ѽ� (����)
- `target`�G�ؼ� iframe �W�� (�i��)
- `loading`�G�O�_��� loading �ʵe (���L�ȡA�i��)

#### �d��

```javascript
$('#myForm').launchDownload('/Download/Export', { extraParam: 'value' }, null, true);
```

#### �`�N�ƶ�
- **Controller Action �����е� `[HttpPost]`**
- **�Ѽƥ����е� `[FromForm]`**

##### �d�� Controller

```csharp
[HttpPost]
public IActionResult Export([FromForm] ExportViewModel model)
{
    // �B�z�U���޿�
}
```

### ��L�\��
- ���ǦC�ơG`$(form).serializeObject()`
- �ɮפW�ǡG`uploadFile($file, postData, url, callback, errorback)`
- �C�L�\��G`$global.printContent($element, printDone)`

---

�p�ݧ�h�Ӹ`�A�аѦ� `WebHome/wwwroot/js/site.js` ��l�X���ѡC