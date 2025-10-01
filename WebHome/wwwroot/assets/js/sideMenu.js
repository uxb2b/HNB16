/**
 * index.html 左側選單資料表及生成/控制單元
 */

// 檢查 auth BH: 經辦，BS:主管，SM:系統管理員，MB:總行，BM:分行管理員
let authType = null; // 預設經辦
const urlSearchParams = new URLSearchParams(window.location.search);
authType = urlSearchParams.get('auth');
const authTitle = document.querySelector('#authTitle');
const todoList = document.querySelector('#todoList');
const info = document.querySelector('#info');
const mainContent = document.querySelector('#main-content');
let currentMenu = [];

// 側邊選單列表 BH: 經辦，BS:主管，SM:系統管理員，MB:總行，BM:分行管理員
const accordionMenu = {
  BH: [    
    {
      id: 'App',
      text: '申請作業',
      sub: [
        { id: 'LcApp', text: '開狀申請書' },
        { id: 'AmendApp', text: '修改申請書' },
        { id: 'DraftApp', text: '押匯申請' },
        { id: 'AmendAccept', text: '修改信用狀接受註記' },
        { id: 'CancelApp', text: '註銷申請/切結書' },
        { 
          id: 'SameDayReversal', 
          text: '當日沖正交易',
          sub: [
            { id: 'AmendLcApp', text: '開狀沖正(EC)' },
            { id: 'CurrentAmendApp', text: '修狀沖正(EC)' },
            { id: 'CurrentCancelApp', text: '註銷信用狀(EC)' },
            { id: 'CurrentAmendDraftApp', text: '押匯沖正(EC)' },
          ]
        },
        { 
          id: 'SameDayAdjustment',
          text: '當日調整帳務',
          sub: [
            { id: 'LcAdjustment', text: '開狀申請當日調整帳務' },
            { id: 'AmendAdjustment', text: '修改申請當日調整帳務' },
            { id: 'CancelAdjustment', text: '註銷申請當日調整帳務' },
            { id: 'DraftAdjustment', text: '押匯申請當日調整帳務' },
          ]
        },        
        { id: 'SentTrfStatus', text: '傳送已轉帳狀態' },
        { id: 'ExpiredLc', text: 'CDS 過期案件主動註銷' },
      ]
    },
    {
      id: 'Review',
      text: '編審作業',
      sub: [
        { id: 'ReviewLcApp', text: '開狀申請書' },
        { id: 'ReviewAmendApp', text: '修改申請書' },
        { id: 'ReviewCancelApp', text: '註銷申請/切結書' },
        { id: 'ReviewDraftApp', text: '押匯申請' },
        { id: 'ReviewInterestRate', text: '利率約定(改貸)' }
      ]
    },
    {
      id: 'Query',
      text: '查詢作業',
      sub: [
        { id: 'QueryLcApp', text: '開狀申請書' },
        { id: 'QueryAmendApp', text: '修改申請書' },
        { id: 'QueryAmendNotice', text: '修改通知書' },
        { id: 'QueryCancelApp', text: '註銷申請/切結書' },
        { id: 'QueryLcList', text: '信用狀' },
        { id: 'QueryDraftList', text: '押匯申請' },
        { id: 'QueryMarLc', text: '主管核准開狀紀錄' },
        { id: 'QueryMarDraft', text: '主管核准押匯紀錄' },
        { id: 'QueryOutstandLc', text: '未結案信用狀額度與保證金' },
        { id: 'QueryInterestRate', text: '利率約定 (改貸)' },
        { id: 'QueryAccount', text: '會計帳務分錄' },
      ]
    },
    {
      id: 'Prompt',
      text: '電子押匯提示作業',
      sub: [
        { id: 'PromptDraft', text: '押匯提示' },
        { id: 'RePromptDraft', text: '重新提示' },
        { id: 'WebPromptDraft', text: '網銀押匯提示' }
      ]
    },
    {
      id: 'Amend',
      text: '補收開狀手續費作業',
      sub: [
        { id: 'ChargeLc', text: '補收開狀手續費' },
        { id: 'ChargeAmendLc', text: '當日更正補收開狀手續費'  }
      ]
    },
    {
      id: 'Customer',
      text: '客戶管理作業',
      sub: [
        { id: 'SetGroup', text: '設定客戶群組' },
        { id: 'SetCustomer', text: '設定客戶資料' },
        { id: 'SetCustomerBranch', text: '客戶所屬分行異動' },
        { id: 'SetBeneficiary', text: '受益人資料' }
      ]
    },
    {
      id: 'Member',
      text: '會員管理作業',
      sub: [
        { id: 'ManagerPersonal', text: '個人設定' }
      ]
    },
  ],
  BS: [
    {
      id: 'App',
      text: '申請作業',
      sub: [
        { id: 'AmendLcApp', text: '當日更正開狀申請書' },
        { id: 'CurrentAmendApp', text: '當日更正修改申請書' },
        { id: 'CurrentCancelApp', text: '當日更正註銷申請/切結書' },
        { id: 'CurrentAmendDraftApp', text: '當日更正押匯申請作業' },
        { id: 'ExpiredLc', text: 'CDS平台-過期信用狀主動註銷' },
        { id: 'LcAdjustment', text: '開狀申請當日調整帳務' },
        { id: 'AmendAdjustment', text: '修改申請當日調整帳務' },
        { id: 'CancelAdjustment', text: '註銷申請當日調整帳務' },
        { id: 'DraftAdjustment', text: '押匯申請當日調整帳務' }
      ]
    },
    {
      id: 'Review',
      text: '編審作業',
      sub: [
        { id: 'ReviewLcApp', text: '開狀申請書' },
        { id: 'ReviewAmendApp', text: '修改申請書' },
        { id: 'ReviewCancelApp', text: '註銷申請/切結書' },
        { id: 'ReviewDraftApp', text: '押匯申請' },
        { id: 'ReviewInterestRate', text: '利率約定(改貸)' }
      ]
    },
    {
      id: 'Query',
      text: '查詢作業',
      sub: [
        { id: 'QueryLcApp', text: '開狀申請書' },
        { id: 'QueryAmendApp', text: '修改申請書' },
        { id: 'QueryAmendNotice', text: '修改通知書' },
        { id: 'QueryCancelApp', text: '註銷申請/切結書' },
        { id: 'QueryLcList', text: '信用狀' },
        { id: 'QueryDraftList', text: '押匯申請' },
        { id: 'QueryMarLc', text: '主管核准開狀紀錄' },
        { id: 'QueryMarDraft', text: '主管核准押匯紀錄' },
        { id: 'QueryOutstandLc', text: '未結案信用狀額度與保證金' },
        { id: 'QueryInterestRate', text: '利率約定 (改貸)' },
        { id: 'QuerySar', text: '主管審核異常' },
        { id: 'QueryAccount', text: '會計帳務分錄' },
      ]
    },
    {
      id: 'Prompt',
      text: '電子押匯提示作業',
      sub: [
        { id: 'PromptDraft', text: '押匯提示' },
        { id: 'RePromptDraft', text: '重新提示' },
        { id: 'WebPromptDraft', text: '網銀押匯提示' }
      ]
    },
    {
      id: 'Amend',
      text: '補收開狀手續費作業',
      sub: [
        { id: 'ChargeLc', text: '補收開狀手續費' },
        { id: 'ChargeAmendLc', text: '當日更正補收開狀手續費'  }
      ]
    },
    {
      id: 'Customer',
      text: '客戶管理作業',
      sub: [
        { id: 'SetGroup', text: '設定客戶群組' },
        { id: 'SetBeneficiary', text: '受益人資料' }
      ]
    },
    {
      id: 'Member',
      text: '會員管理作業',
      sub: [
        { id: 'ManagerPersonal', text: '個人設定' }
      ]
    },
  ],
  SM: [
    {
      id: 'Query',
      text: '查詢作業',
      sub: [
        { id: 'QueryAccessLog', text: '存取記錄' }
      ]
    },
    {
      id: 'Prompt',
      text: '電子押匯提示作業',
      sub: [
        { id: 'DraftRejection', text: '押匯拒絕原因維護' }
      ]
    },
    {
      id: 'Member',
      text: '會員管理作業',
      sub: [
        { id: 'ManagerBranch', text: '分行管理' },
        { id: 'ManagerMember', text: '會員管理' },
        { id: 'ManagerPersonal', text: '個人設定' }
      ]
    },
  ],
  MB: [
    {
      id: 'Query',
      text: '查詢作業',
      sub: [
        { id: 'QueryLcApp', text: '開狀申請書' },
        { id: 'QueryAmendApp', text: '修改申請書' },
        { id: 'QueryAmendNotice', text: '修改通知書' },
        { id: 'QueryCancelApp', text: '註銷申請/切結書' },
        { id: 'QueryLcList', text: '信用狀' },
        { id: 'QueryDraftList', text: '押匯申請' },
        { id: 'QueryMarLc', text: '主管核准開狀紀錄' },
        { id: 'QueryMarDraft', text: '主管核准押匯紀錄' },
        { id: 'QueryOutstandLc', text: '未結案信用狀額度與保證金' },
        { id: 'QueryBlcAmount', text: '分行信用狀暨押匯筆數金額' },
        { id: 'QueryInterestRate', text: '利率約定 (改貸)' },
        { id: 'QueryAccount', text: '會計帳務分錄' },
        { id: 'QueryAccessLog', text: '存取記錄' },
        { id: 'QueryNegotiationFee', text: '國內信用狀押匯手續費優待明細表' },
        { id: 'QueryBeneficiaryBlcAmount', text: '受益人分行信用狀暨押匯筆數金額' },
        { id: 'QueryClientConnect', text: 'Client連線狀態' }
      ]
    },
    {
      id: 'Member',
      text: '會員管理作業',
      sub: [
        { id: 'ManagerPersonal', text: '個人設定' },
        { id: 'ManagerBeneficiary', text: '受益人資料維護' }
      ]
    },
  ],
  BM: [
    {
      id: 'Member',
      text: '會員管理作業',
      sub: [
        { id: 'ManagerMember', text: '會員管理' },
        { id: 'ManagerPersonal', text: '個人設定' },
      ]
    },
  ],
};

switch(authType) {
  case 'BH':
    authTitle.textContent = '經辦';
    currentMenu = accordionMenu.BH;
    break;
  case 'BS':
    authTitle.textContent = '主管';
    currentMenu = accordionMenu.BS;
    break;
  case 'SM':
    authTitle.textContent = '系統管理員';
    currentMenu = accordionMenu.SM;
    todoList.classList.add('hnb__hide');
    mainContent.src = './pages/Home.html';
    break;
  case 'MB':
    authTitle.textContent = '總行';
    currentMenu = accordionMenu.MB;
    info.classList.add('hnb__hide');
    todoList.classList.add('hnb__hide');
    mainContent.src = './pages/Home.html';
    break;
  case 'BM':
    authTitle.textContent = '分行管理員';
    currentMenu = accordionMenu.BM;
    info.classList.add('hnb__hide');
    todoList.classList.add('hnb__hide');
    mainContent.src = './pages/Home.html';
    break;
};

const accordionSideMenu = document.querySelector('#accordionSideMenu');
// 建立側選單-依登入權限找到對應選單資料並生成選單
currentMenu.forEach(accordion => {
  // 建立第一層手風琴選單
  const accordionItem = document.createElement('div');
  accordionItem.classList.add('accordion-item');
  accordionItem.innerHTML = `
  <h2 class="accordion-header" id="heading_${accordion.id}">
    <button class="py-2 fs-1-5 fw-bold accordion-button hnb__menu--title rounded-0 collapsed" type="button"
      data-bs-toggle="collapse" data-bs-target="#${accordion.id}" aria-expanded="false" aria-controls="${accordion.id}">
      ${accordion.text}
    </button>
  </h2>
  <div id="${accordion.id}" class="accordion-collapse collapse" aria-labelledby="heading_${accordion.id}"
    data-bs-parent="#accordionSideMenu">
    <div class="accordion-body p-0">
      <div class="list-group list-group-flush" id="list-tab" role="tablist"></div>
    </div>
  </div>
  `;

  // 第一層-找到 id="list-tab" 並將選項加入群組
  const listGroup = accordionItem.childNodes[3].childNodes[1].childNodes[1];

  // 建立第二層手風琴選單
  const subAccordionItem = document.createElement('div');
  subAccordionItem.classList.add('accordion', 'accordion-flush');
  subAccordionItem.id = `${accordion.id}-sub`;
  accordion.sub.forEach(item => {
    if (item.sub) {
      const subAccordionContent = document.createElement('div');
      subAccordionContent.classList.add('accordion-item', 'border-0');
      subAccordionContent.innerHTML = `
      <h3 class="px-2 accordion-header" id="heading_${item.id}">
      <button class="py-2 ps-3 fs-0-8-75 fw-bold subAccordion accordion-button hnb__menu--title rounded-0 border-top border-bottom collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#${item.id}" aria-expanded="false" aria-controls="${item.id}">${item.text}</button>        
      </h3>
      <div id="${item.id}" class="px-2 accordion-collapse collapse" aria-labelledby="heading_${item.id}" data-bs-parent="#${accordion.id}-sub">
      <div class="accordion-body p-0 border-bottom"><div class="list-group list-group-flush" id="list-tab" role="tablist"></div></div></div>
      `;
      // 第二層-找到 id="list-tab" 並將選項加入群組      
      const subListGroup = subAccordionContent.childNodes[3].childNodes[1].childNodes[0];
      item.sub.forEach(subItem => {
        const subItemEl = document.createElement('a');
        subItemEl.classList.add('ps-4-5', 'list-group-item', 'list-group-item-action', 'hnb__menu--btn');
        subItemEl['id'] = `${subItem.id}`;
        subItemEl['href'] = 'javascript:void(0)';
        subItemEl['role'] = 'tab';
        subItemEl.setAttribute('data-bs-toggle', 'list');
        subItemEl.setAttribute('aria-controls', `list-${subItem.id}`);
        subItemEl.textContent = `${subItem.text}`;
        subItemEl.addEventListener('click', () => {
          loadPage(accordion.id, subItem.id);
        });
        subListGroup.appendChild(subItemEl);
        subAccordionItem.appendChild(subAccordionContent);
      });

      listGroup.appendChild(subAccordionItem);
    } else {
      const itemEl = document.createElement('a');
      itemEl.classList.add('ps-4', 'list-group-item', 'list-group-item-action', 'hnb__menu--btn');
      itemEl['id'] = `${item.id}`;
      itemEl['href'] = 'javascript:void(0)';
      itemEl['role'] = 'tab';
      itemEl.setAttribute('data-bs-toggle', 'list');
      itemEl.setAttribute('aria-controls', `list-${item.id}`);
      itemEl.textContent = `${item.text}`;
      itemEl.addEventListener('click', () => {
        loadPage(accordion.id, item.id);
      });
      listGroup.appendChild(itemEl);
    };

  });
  accordionSideMenu.appendChild(accordionItem);
});

// 主選單列表
const accordionList = ['Amend','App','Review','Query','Prompt','Customer','Member'];
// 次選單列表
const subAccordionList = ['SameDayReversal', 'SameDayAdjustment'];
// 頁面載入後執行
window.addEventListener('load', () => {
  initMenu();
});

function initMenu() {
  // 表單初始化
  clearAccordionAction();
  clearSubAccordionAction();
  changeAccordion(accordionList, 'main');
  changeAccordion(subAccordionList, 'sub');
};

// accordionSideMenu 切換監聽
function changeAccordion(list, level) {
  list.forEach(accordion => {
    const tab = document.querySelector(`button[data-bs-target="#${accordion}"]`);
    // 監聽連結
    if (tab) {
      tab.addEventListener('click', (event) => {
        event.preventDefault();
        const hasCollapsed = tab.classList.contains('collapsed');    
        
        // 樣式切換
        if(!hasCollapsed) {        
          if (level === 'main') {
            clearAccordionAction();
          }
          clearSubAccordionAction();
          tab.classList.remove('collapsed');
        } else {
          tab.classList.add('collapsed');
        }
      });
    }
  });
};

// accordionSideMenu 切換監聽
subAccordionList.forEach(accordion => {
  const tab = document.querySelector(`button[data-bs-target="#${accordion}"]`);
  // 監聽連結
  if (tab) {
    tab.addEventListener('click', (event) => {
      event.preventDefault();
      const hasCollapsed = tab.classList.contains('collapsed');    
      
      // 樣式切換
      if(!hasCollapsed) {        
        clearSubAccordionAction();
        tab.classList.remove('collapsed');
      } else {
        tab.classList.add('collapsed');
      }
    });
  }
});

// 收闔第一層手風琴選單
function clearAccordionAction() {
  accordionList.forEach(accordion => {
    const tabItem = document.querySelector(`button[data-bs-target="#${accordion}"]`);
    const tabContent = document.querySelector(`#${accordion}`);
    if (tabItem) {
      const hasCollapsed = tabItem.classList.contains('collapsed');
      if (!hasCollapsed) {
        tabItem.classList.add('collapsed');
      }
      tabContent.classList.remove('show');
    }
  });
};

// 收闔第二層手風琴選單
function clearSubAccordionAction() {
  subAccordionList.forEach(accordion => {
    const tabItem = document.querySelector(`button[data-bs-target="#${accordion}"]`);
    const tabContent = document.querySelector(`#${accordion}`);
    if (tabItem) {
      const hasCollapsed = tabItem.classList.contains('collapsed');
      if (!hasCollapsed) {
        tabItem.classList.add('collapsed');
      }
      tabContent.classList.remove('show');
    }
  });
};

// 監聽側邊選單連結
const menuList = document.querySelectorAll('.list-group-item');
menuList.forEach(menuItem => {  
  // 監聽連結
  if (menuItem) {
    menuItem.addEventListener('click', (event) => {
      event.preventDefault();
    
       // 樣式切換
       clearAction();
       event.target.classList.add('active');  
     });
   }
 });

// 清除選單全部 [.active] 樣式
function clearAction() {
  menuList.forEach(menu => {
    menu.classList.remove('active');
  });
};

// 載入對應的頁面
function loadPage(menu, subMenu) {  
  document.querySelector('#main-content').src = `./pages/${menu}/${subMenu}.html`;
};