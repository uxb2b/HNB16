// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var $global = (function () {
    var $printContainer = {};

    function doPrint2023($element, printDone) {

        $global.preview = window.open('', '_blank', 'popup');
        $element.find('style').attr('nonce', 'Y2hiY29kZQ==');
        $global.preview.document.write($element.html());
        $global.preview.document.write('<script nonce="Y2hiY29kZQ==">window.print();\<\/script>');

        setTimeout(function () {
            if (typeof printDone === "function") {
                printDone();
            }
        }, 3000);

    }

    function doOpenToPrint(html, printDone) {

        $global.preview = window.open('', '_blank', 'popup');
        $global.preview.document.write(html);
        $global.preview.document.write('<script>window.print();\<\/script>');

        setTimeout(function () {
            if (typeof printDone === "function") {
                printDone();
            }
        }, 3000);

    }

    function doPrint($element, printDone) {

        if ($printContainer.$prnFrame) {
            $printContainer.$prnFrame.remove();
        }

        var strFrameName = ("printer-" + (new Date()).getTime());
        var jFrame = $("<iframe name='" + strFrameName + "'>");

        jFrame
            .css("width", "1px")
            .css("height", "1px")
            .css("position", "absolute")
            .css("display", "none")
            .appendTo($("body:first"));

        var objFrame = window.frames[strFrameName];
        $printContainer.$prnFrame = jFrame;

        var objDoc = objFrame.document;
        objDoc.write($printContainer.documentTemplate);
        var container = null;
        var count = 0;
        while (container == null && count < 100) {
            container = objDoc.getElementById('printContainer');
            count++;
        }

        if (container == null) {
            alert('列印失敗!! 請重試...');
        }

        $element.find('style').attr('nonce', 'Y2hiY29kZQ==');
        var $nodes = $.parseHTML($element.html(), objDoc, true);
        $nodes.forEach(function (node, index) {
            if (node.nodeName.toLowerCase() == 'script') {
                var s = objDoc.createElement('script');
                s.innerText = node.innerText;
                container.appendChild(s);
            } else {
                container.appendChild(node);
            }
        });

        objFrame.focus();
        setTimeout(function () {
            objFrame.print();
            if (typeof printDone === "function") {
                printDone();
            }
        }, 3000);

    }

    function doPrintHtml(html, printDone) {

        var old = document.querySelector('iframe[name="prnFrame"]');
        if (old) {
            old.remove();
        }

        var jFrame = $("<iframe name='prnFrame'>");

        jFrame
            .css("width", "1px")
            .css("height", "1px")
            .css("position", "absolute")
            .css("display", "none")
            .appendTo($("body:first"));

        $printContainer.objFrame = window.frames['prnFrame'];
        $printContainer.objDoc = $printContainer.objFrame.document;

        $printContainer.objDoc.write(html);

        $printContainer.objFrame.focus();
        setTimeout(function () {
            $printContainer.objFrame.print();
            if (typeof printDone === "function") {
                printDone();
            }
        }, 3000);

    }

    return {
        registerCloseEvent: function ($tab) {
            $tab.find(".closeTab").click(function () {

                //there are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
                var tabContentId = $(this).parent().attr("href");
                $(this).parent().parent().remove(); //remove li of tab
                $('#masterTab a:last').tab('show'); // Select first tab
                $(tabContentId).remove(); //remove respective tab content

            });
        },
        removeTab: function (tabId) {
            var $a = $('#masterTab a[href="#' + tabId + '"]');
            $a.parent().remove(); //remove li of tab
            $('#masterTab a:last').tab('show'); // Select first tab
            $('#' + tabId).remove(); //remove respective tab content
        },
        showTab: function (tabId) {
            $('#masterTab a[href="#' + tabId + '"]').tab('show');
        },
        createTab: function (tabId, tabText, tabContent, show) {
            showLoading();
            this.removeTab(tabId);

            var newTab = $('<li role="presentation"></li>')
                .append($('<a href="#masterHome" class="tab-link" role="tab" data-toggle="tab"></a>')
                    .attr('href', '#' + tabId).attr('aria-controls', tabId).text(tabText)
                    .append($('<button class="close closeTab"><i class="fa fa-times" aria-hidden="true"></i></button>')));
            newTab.appendTo($('#masterTab'));
            $('<div role="tabpanel" class="tab-pane"></div>').attr('id', tabId)
                .append(tabContent).appendTo($('#masterTabContent'));
            this.registerCloseEvent(newTab);
            if (show)
                this.showTab(tabId);
            hideLoading();
        },
        onReady: [],
        nothingToken: false,
        doNothing: function () {
            this.nothingToken = true;
            $.post('../Home/SystemInfo', null, function (data) {
                if ($.isPlainObject(data)) {
                    console.log(data);
                }
                if ($global.nothingToken) {
                    setTimeout($global.doNothing, 30000);
                }
            });
        },
        printContent: function ($element, printDone) {
            if ($printContainer == undefined) {
                showLoading();
                $.post('../Helper/PrintPage', {}, function (data) {
                    hideLoading();
                    if ($.isPlainObject(data)) {
                        alertModal("無法取得列印文件!!");
                    } else {
                        $printContainer = { 'documentTemplate': data };
                        doPrint($element, printDone);
                    }
                });
            } else {
                doPrint($element, printDone);
            }
        },
        printHtml: function (html, printDone) {
            doPrintHtml(html, printDone);
        },
        openToPrintHtml: function (html, printDone) {
            doOpenToPrint(html, printDone);
        },
        getPrintArea: function ($element) {
            var $printArea = $element.find('#printArea');
            if ($printArea.length > 0) {
                return $printArea;
            }

            var $div = $('<div></div>').append($element);
            $printArea = $div.find('#printArea');
            if ($printArea.length > 0) {
                return $printArea;
            } else {
                return $element;
            }
        },
    };
})();

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.launchDownload = function (url, params, target, loading) {

    var data = this.serializeObject();
    if (params) {
        $.extend(data, params);
    }

    if (loading) {
        token = (new Date()).getTime();
        data.FileDownloadToken = token;
    }

    var form = $('<form></form>').attr('action', url).attr('method', 'post');//.attr('target', '_blank');
    if (target) {
        form.attr('target', target);
        if (window.frames[target] == null) {
            $('<iframe>')
                .css('display', 'none')
                .attr('name', target).appendTo($('body'));
        }
    }

    Object.keys(data).forEach(function (key) {
        var value = data[key];

        if (value instanceof Array) {
            value.forEach(function (v) {
                form.append($("<input></input>").attr('type', 'hidden').attr('name', key).attr('value', v));
            });
        } else {
            form.append($("<input></input>").attr('type', 'hidden').attr('name', key).attr('value', value));
        }

    });

    if (loading) {
        showLoading();
        fileDownloadCheckTimer = window.setInterval(function () {
            var cookieValue = $.cookie('FileDownloadToken');
            if (cookieValue == token)
                finishDownload();
        }, 1000);
    }

    //send request
    form.appendTo('body').submit().remove();
};

function finishDownload() {
    window.clearInterval(fileDownloadCheckTimer);
    $.removeCookie('fileDownloadToken'); //clears this cookie value
    hideLoading();
}


function initSort(sort, offset) {

    $('.itemList th').each(function (idx, elmt) {
        var $this = $(this);
        if (sort.indexOf(idx + offset) >= 0) {
            $this.attr('aria-sort', 'ascending');
            $this.append('<i class="fa fa-sort-asc" aria-hidden="true"></i>')
                .append($('<input type="hidden" name="sort"/>').val(idx + offset));
        } else if (sort.indexOf(-idx - offset) >= 0) {
            $this.attr('aria-sort', 'desending');
            $this.append('<i class="fa fa-sort-desc" aria-hidden="true"></i>')
                .append($('<input type="hidden" name="sort"/>').val(-idx - offset));
        }
    });
}

const defaultDateConfig = {
    locale: 'zh_tw',
    //enableTime: true, //可選時與分
    dateFormat: "Y/m/d", //時間格式
    time_24hr: true, //24 時制
    minuteIncrement: 15, //分鐘每次選擇間隔單位
    allowInput: true, //可輸入控制
    //minDate: "today", //可選最小時間，可直接接受 'today' 字串
    //maxDate: currentTime.setMonth(currentTime.getMonth() + 1), //可選最大時間，從今天起一個月
    onChange: function (selectedDates, dateStr, instance) {
    }
};
function datepicker(items, options) {
    if (options) {
        options = Object.assign({}, defaultDateConfig, options);
    }
    flatpickr(items, options);
}

$(function () {
    datepicker(document.querySelectorAll('input[type="datetime-local"]'));
});

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    _title: function (title) {
        if (!this.options.title) {
            title.html("&#160;");
        } else {
            title.html(this.options.title);
        }
    }
}));

function uploadFile($file, postData, url, callback, errorback) {

    $('<form method="post" enctype="multipart/form-data"></form>')
        .append($file).ajaxForm({
            url: url,
            data: postData,
            beforeSubmit: function () {
                showLoading();
            },
            success: function (data) {
                hideLoading();
                callback(data);
            },
            error: function () {
                hideLoading();
                errorback();
            }
        }).submit();
}

function clearErrors() {
    $('input.error,select.error,textarea.error').removeClass('error');
    $('label.error').remove();
}

function alertModal(alertMessage) {
    $('<div>' + alertMessage + '</div>')
        .dialog({ "title": "<div class='modal-title'><h4><i class='fa fa-clock-o'></i>訊息</h4></div>" });
}

function ajaxSubmitForm(form) {
    $(form).ajaxSubmit({
        beforeSubmit: function () {
            showLoading();
        },
        success: function (data) {
            hideLoading();
            if ($.isPlainObject(data)) {
                if (form.done) {
                    form.done(data);
                } else if (data.message) {
                    alert(data.message);
                }
            } else {
                if (form.done) {
                    form.done(data);
                } else {
                    $(data).appendTo($('body'));
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            hideLoading();
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
}

function doPost(url, viewModel, done) {
    showLoading();
    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(viewModel),
        type: "POST",
        //dataType: "json",
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            hideLoading();
            if ($.isPlainObject(data)) {
                if (data.result) {
                    if (done) {
                        done(data);
                    }
                } else {
                    alert(data.message);
                }
            } else {
                if (done) {
                    done(data);
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            hideLoading();
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
}

function doFetch(url, options = {}) {
    const defaultOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8'
        }
    };

    const fetchOptions = { ...defaultOptions, ...options };

    if (fetchOptions.body && typeof fetchOptions.body !== 'string') {
        fetchOptions.body = JSON.stringify(fetchOptions.body);
    }

    return fetch(url, fetchOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP 錯誤: ${response.status}`);
            }

            const contentType = response.headers.get("content-type");

            if (contentType && contentType.includes("application/json")) {
                return response.json();
            } else {
                return response.text();
            }
        });
}

function toArrayField(viewModel, field) {
    var result = [];
    if (viewModel[field] instanceof Array) {
        result = viewModel[field];
    } else {
        result.push(viewModel[field]);
        viewModel[field] = result;
    }
    return result;
}

function checkTableAll(itemName) {
    itemName = itemName || 'chkItem';
    var event = event || window.event;  // || arguments.callee.caller.arguments[0];
    var $this = $(event.target);
    $this.closest('table').find('input[name=\'' + itemName + '\']').prop('checked', $this.is(':checked'));
}