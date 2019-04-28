
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
function setURLParam(url, para_name, para_value) {
    var strNewUrl = new String();
    var strUrl = url.replace("#", "");
    if (strUrl.indexOf("?") != -1) {
        strUrl = strUrl.substr(strUrl.indexOf("?") + 1);
        if (strUrl.toLowerCase().indexOf(para_name.toLowerCase()) == -1) {
            strNewUrl = url + "&" + para_name + "=" + para_value;
            return strNewUrl;
        } else {
            var aParam = strUrl.split("&");
            for (var i = 0; i < aParam.length; i++) {
                if (aParam[i].substr(0, aParam[i].indexOf("=")).toLowerCase() == para_name.toLowerCase()) {
                    aParam[i] = aParam[i].substr(0, aParam[i].indexOf("=")) + "=" + para_value;
                }
            }

            strNewUrl = url.substr(0, url.indexOf("?") + 1) + aParam.join("&");
            return strNewUrl;
        }

    } else {
        strUrl += "?" + para_name + "=" + para_value;
        return strUrl
    }
} function setURLParam(url, para_name, para_value) {
    var strNewUrl = new String();
    var strUrl = url.replace("#", "");
    if (strUrl.indexOf("?") != -1) {
        strUrl = strUrl.substr(strUrl.indexOf("?") + 1);
        if (strUrl.toLowerCase().indexOf(para_name.toLowerCase()) == -1) {
            strNewUrl = url + "&" + para_name + "=" + para_value;
            return strNewUrl;
        } else {
            var aParam = strUrl.split("&");
            for (var i = 0; i < aParam.length; i++) {
                if (aParam[i].substr(0, aParam[i].indexOf("=")).toLowerCase() == para_name.toLowerCase()) {
                    aParam[i] = aParam[i].substr(0, aParam[i].indexOf("=")) + "=" + para_value;
                }
            }

            strNewUrl = url.substr(0, url.indexOf("?") + 1) + aParam.join("&");
            return strNewUrl;
        }

    } else {
        strUrl += "?" + para_name + "=" + para_value;
        return strUrl
    }
}

function delUrlParam(url, name) {
    var i = url;
    var reg = new RegExp("([&\?]?)" + name + "=[^&]+(&?)", "g")

    var newUrl = i.replace(reg, function (a, b, c) {
        if (c.length == 0) {
            return '';
        } else {
            return b;
        }
    });
    return newUrl;
}

function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
function getChk() {
    var CheckedValue = "";
    $("input[type=checkbox][name=chkID]:checked").each(function () {
        CheckedValue += $(this).val() + ",";
    })
    return CheckedValue;
}
$(function () {

    $("#chkAll").click(function () {
        $("input[type=checkbox][name=chkID]").each(function () {
            $(this)[0].checked = $("#chkAll")[0].checked;
        })
    })
})
//判断是否支持html5 视频标签
function checkVideo() {
    if (!!document.createElement('video').canPlayType) {
        var vidTest = document.createElement("video");
        oggTest = vidTest.canPlayType('video/ogg; codecs="theora, vorbis"');
        if (!oggTest) {
            h264Test = vidTest.canPlayType('video/mp4; codecs="avc1.42E01E, mp4a.40.2"');
            if (!h264Test) {
                return false;
            }
            else {
                if (h264Test == "probably") {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        else {
            if (oggTest == "probably") {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else {
        return false;
    }
}

function getDateDiff(dateTimeStamp) {
    debugger
    //JavaScript函数：
    var seconde = 1000 * 60*60;
    var minute = 1000 * 60;
    var hour = minute * 60;
    var day = hour * 24;
    var halfamonth = day * 15;
    var month = day * 30;
    var year = month * 12;
    var now = new Date().getTime();

    var diffValue = now - dateTimeStamp;
    if (diffValue < 0) {
        //若日期不符则弹出窗口告之
        //alert("结束日期不能小于开始日期！");
        return "";
    }
    var yearC = diffValue / year;
    var monthC = diffValue / month;
    var weekC = diffValue / (7 * day);
    var dayC = diffValue / day;
    var hourC = diffValue / hour;
    var minC = diffValue / minute;
    var secondC = diffValue / seconde;
    if (yearC >= 1) {
        result = parseInt(yearC) + "个年前";
    }
    else if (monthC >= 1) {
        result = parseInt(monthC) + "个月前";
    }
    else if (weekC >= 1) {
        result = parseInt(weekC) + "周前";
    }
    else if (dayC >= 1) {
        result = parseInt(dayC) + "天前";
    }
    else if (hourC >= 1) {
        result = parseInt(hourC) + "个小时前";
    }
    else if (minC >= 1) {
        result = parseInt(minC) + "分钟前";
    } else if (secondC >= 1) {
        result = parseInt(secondC) + "秒";
    } else
        result = "刚刚";
    return result;
}
function showImg(title, path) {
    layer.open({
        type: 1,
        title: title,
        maxmin: true,
        shadeClose: true, //开启遮罩关闭
        content: "<img src='" + path + "'>"
    });
}
function showImgWh(title, path,w,h) {
    layer.open({
        type: 1,
        title: title,
        area: [w, h], 
        maxmin: true,
        shadeClose: true, //开启遮罩关闭
        content: "<img src='" + path + "'>"
    });
}