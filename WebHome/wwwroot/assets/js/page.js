// 檢查 auth BH: 經辦，BS:主管，SM:系統管理員，MB:總行，BM:分行管理員
let authType = null; // 預設經辦
function getAuth() {
  const urlSearchParams = new URLSearchParams(parent.location.search);
  authType = urlSearchParams.get('auth');  
}
getAuth();


function backHomeHandler() {
  // 初始化 Accordion
  window.parent.clearAccordionAction();
  // 初始化選項 active，再指定項目
  window.parent.clearAction();
  location.href="../../pages/Home.html";
};

const ln441 = [
  { value: '0', text: '固定利率'},
  { value: '3', text: '隨質借存單利率浮動'},
  { value: '4', text: '隨金融債券利率浮動'},
  { value: '5', text: '隨定儲利率指數按季浮動'},
  { value: '6', text: '隨基準利率按季浮動'},
  { value: '7', text: '51328頁初級市場利率固定計息'},
  { value: '8', text: '隨51328頁初級市場利率定期浮動計息'},
  { value: '9', text: '隨郵儲金一年期浮動利率隨時浮動'},
  { value: 'A', text: '隨郵儲金二年期浮動利率隨時浮動'},
  { value: 'B', text: '隨郵匯局中長期資金運用利率隨時浮動'},
  { value: 'C', text: '隨本行六個月定期存款固定利率隨時浮動'},
  { value: 'D', text: '隨本行六個月定期存款機動利率隨時浮動'},
  { value: 'E', text: '隨台企銀基放利率隨時浮動'},
  { value: 'F', text: '隨土銀基放利率隨時浮動'},
  { value: 'G', text: '隨農銀基放利率隨時浮動'},
  { value: 'H', text: '隨本行一年期定期存款機動利率隨時浮動'},
  { value: 'I', text: '隨本行二年期定期存款機動利率隨時浮動'},
  { value: 'J', text: '隨本行一年期定期儲蓄存款機動利率隨時浮動'},
  { value: 'K', text: '隨本行二年期定期儲蓄存款機動利率隨時浮動'},
  { value: 'L', text: '隨本行活期存款機動利率隨時浮動'},
  { value: 'M', text: '隨本行活期儲蓄存款機動利率隨時浮動'},
  { value: 'N', text: '隨其它利率定期浮動'},
  { value: 'P', text: '隨定儲利率指數按月浮動'},
  { value: 'Q', text: '隨基準利率按月浮動'},
  { value: 'R', text: '隨本行一年期定期存款固定利率隨時浮動'},
  { value: 'S', text: '隨本行二年期定期存款固定利率隨時浮動'},
  { value: 'T', text: '隨本行一年期定期儲蓄存款固定利率隨時浮動'},
  { value: 'U', text: '隨本行二年期定期儲蓄存款固定利率隨時浮動'},
  { value: 'V', text: '6165頁次級市場利率固定計息'},
  { value: 'W', text: '隨6165頁次級市場利率定期浮動計息'},
  { value: 'X', text: '隨TAIBOR利率固定計息 (TAIBOR同天期)'},
  { value: 'Y', text: '隨TAIBOR利率定期浮動計息(TAIBOR同天期,每N個月浮動)'},
  { value: 'Z', text: '行員貸款'},
];

const irr = document.querySelector('#irr');
if (irr) {
  ln441.forEach((item, index) => {
    const el = document.createElement('option');
    el.value = item.value;
    el.textContent = item.text;
    if (index === 0) {
      el.setAttribute('selected', true);
    }
    irr.appendChild(el);
  });
};