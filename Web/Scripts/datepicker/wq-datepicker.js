var rfixYMD = /^(\d+)\D(\d+)\D(\d+)/

var DATE_FORMATS = {
    yyyy: dateGetter("FullYear", 4),
    yy: dateGetter("FullYear", 2, 0, true),
    y: dateGetter("FullYear", 1),
    MMMM: dateStrGetter("Month"),
    MMM: dateStrGetter("Month", true),
    MM: dateGetter("Month", 2, 1),
    M: dateGetter("Month", 1, 1),
    dd: dateGetter("Date", 2),
    d: dateGetter("Date", 1),
    HH: dateGetter("Hours", 2),
    H: dateGetter("Hours", 1),
    hh: dateGetter("Hours", 2, -12),
    h: dateGetter("Hours", 1, -12),
    mm: dateGetter("Minutes", 2),
    m: dateGetter("Minutes", 1),
    ss: dateGetter("Seconds", 2),
    s: dateGetter("Seconds", 1),
    sss: dateGetter("Milliseconds", 3),
    EEEE: dateStrGetter("Day"),
    EEE: dateStrGetter("Day", true),
    a: ampmGetter,
    Z: timeZoneGetter
}

var dateFormat = function (date, format) {
    var text = "",
            parts = [],
            fn, match
    format = format || "mediumDate"
    format = locate[format] || format
    if (typeof date === "string") {
        if (NUMBER_STRING.test(date)) {
            date = toInt(date)
        } else {
            var trimDate = date.trim()
            date = trimDate.replace(rfixYMD, function (a, b, c, d) {
                var array = d.length === 4 ? [d, b, c] : [b, c, d]
                return array.join("-")
            })
            date = jsonStringToDate(date)
        }
        date = new Date(date)
    }
    if (typeof date === "number") {
        date = new Date(date)
    }
    if (gettype(date) !== "date") {
        return
    }
    while (format) {
        match = /((?:[^yMdHhmsaZE']+)|(?:'(?:[^']|'')*')|(?:E+|y+|M+|d+|H+|h+|m+|s+|a|Z))(.*)/.exec(format)
        if (match) {
            parts = parts.concat(match.slice(1))
            format = parts.pop()
        } else {
            parts.push(format)
            format = null
        }
    }
    parts.forEach(function (value) {
        fn = DATE_FORMATS[value]
        text += fn ? fn(date, locate) : value.replace(/(^'|'$)/g, "").replace(/''/g, "'")
    });
    return text == "undefined" ? "" : text;
}

function padNumber(num, digits, trim) {
    var neg = ""
    if (num < 0) {
        neg = '-'
        num = -num
    }
    num = "" + num
    while (num.length < digits)
        num = "0" + num
    if (trim)
        num = num.substr(num.length - digits)
    return neg + num
}

function dateGetter(name, size, offset, trim) {
    return function (date) {
        var value = date["get" + name]()
        if (offset > 0 || value > -offset)
            value += offset
        if (value === 0 && offset === -12) {
            value = 12
        }
        return padNumber(value, size, trim)
    }
}

function timeZoneGetter(date) {
    var zone = -1 * date.getTimezoneOffset()
    var paddedZone = (zone >= 0) ? "+" : ""
    paddedZone += padNumber(Math[zone > 0 ? "floor" : "ceil"](zone / 60), 2) + padNumber(Math.abs(zone % 60), 2)
    return paddedZone
}

function ampmGetter(date, formats) {
    return date.getHours() < 12 ? formats.AMPMS[0] : formats.AMPMS[1]
}

function dateStrGetter(name, shortForm) {
    return function (date, formats) {
        var value = date["get" + name]()
        var get = (shortForm ? ("SHORT" + name) : name).toUpperCase()
        return formats[get][value]
    }
}

function gettype(obj) {
    if (obj == null) {
        return String(obj)
    }
    var serialize = Object.prototype.toString;
    return typeof obj === "object" || typeof obj === "function" ?
            class2type[serialize.call(obj)] || "object" :
            typeof obj;
}

var class2type = {}
"Boolean Number String Function Array Date RegExp Object Error".replace(/[^, ]+/g, function (name) {
    class2type["[object " + name + "]"] = name.toLowerCase()
})

var locate = {
    AMPMS: {
        0: "上午",
        1: "下午"
    },
    DAY: {
        0: "星期日",
        1: "星期一",
        2: "星期二",
        3: "星期三",
        4: "星期四",
        5: "星期五",
        6: "星期六"
    },
    MONTH: {
        0: "1月",
        1: "2月",
        2: "3月",
        3: "4月",
        4: "5月",
        5: "6月",
        6: "7月",
        7: "8月",
        8: "9月",
        9: "10月",
        10: "11月",
        11: "12月"
    },
    SHORTDAY: {
        "0": "周日",
        "1": "周一",
        "2": "周二",
        "3": "周三",
        "4": "周四",
        "5": "周五",
        "6": "周六"
    },
    fullDate: "y年M月d日EEEE",
    longDate: "y年M月d日",
    medium: "yyyy-M-d H:mm:ss",
    mediumDate: "yyyy-M-d",
    mediumTime: "H:mm:ss",
    "short": "yy-M-d ah:mm",
    shortDate: "yy-M-d",
    shortTime: "ah:mm"
}

var WqDatepicker = (function ($) {
	/*定义模板*/
	var WRAPPER = "<div></div>",
		calTmp = '<div class="dp-date"></div>',
		HEADBAR = '<div class="dp-headbar"></div>',
		/*顶部的操作栏*/
		tableTmp = '<table class="dp-table" width="100%" border="0" cellspacing="0" cellpadding="0">' +
		'<thead></thead>' +
		'<tbody></tbody>' +
		'</table>',
		CURMONTH = '<span class="dp-curmonth"></span>',
		MONTHSLE = '<div class="dp-sel month clearfix">' +
		'<div class="dp-sel-item">一月</div>' +
		'<div class="dp-sel-item">七月</div>' +
		'<div class="dp-sel-item">二月</div>' +
		'<div class="dp-sel-item">八月</div>' +
		'<div class="dp-sel-item">三月</div>' +
		'<div class="dp-sel-item">九月</div>' +
		'<div class="dp-sel-item">四月</div>' +
		'<div class="dp-sel-item">十月</div>' +
		'<div class="dp-sel-item">五月</div>' +
		'<div class="dp-sel-item">十一</div>' +
		'<div class="dp-sel-item">六月</div>' +
		'<div class="dp-sel-item">十二</div>' +
		'</div>',
		CURYEAR = '<span class="dp-curyear"></span>',
		/*按年or月前后翻页*/
		ICON_TURN_MONTH = '<a class="dp-pre month"></a><a class="dp-next month"></a>',
		ICON_TURN_YEAR = '<a class="dp-pre year"></a><a class="dp-next year"></a>',
		weekBarTmp = '<tr class="dp-table-title" align="center"><td>一</td><td>二</td><td>三</td><td>四</td><td>五</td><td>六</td><td>日</td></tr>',
		/*星期栏*/
		cellTmp = '<td class="dp-table-wday">val</td>',
		/*日期区域
		 * .WotherDay 非当前月日期
		 * .Wwday 当前月日期
		 * .wwClassName 周末
		 * */
		cellClassName = "dp-table-wday",
		otherClassName = "dp-table-otherday",
		wwClassName = "dp-table-wwday",
		footBarTmp, /*底部操作栏*/
		MONTH = ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一", "十二"],
		DEFAULTS = {
			today: new Date(),
			year: true,
			turnyear: true,
			turnmonth: true,
			otherdays: true,
			multi: false,
			click2selected: false,
			onClick: false,
			container: null,
			wholeYear: false,
			onChangeYear:false
		},
		TODAY = dateFormat(new Date(), "yyyy-MM-dd");
		
	/*处理数据：根据某一天生成当月6*7行的日期数据
		1.获取当月的天数
		2.第一天星期几，最后一天星期几。
		3.获取上月，下月的天数补齐数据*/
	var Calendar = function(options) {
		var self = this;
		self.options = $.extend({}, options);
		self.init();
	};
	var _calPType = Calendar.prototype,
		_datePtype = Date.prototype;
	/**
	 * 获取月份的天数
	 * @param  {[type]} month [实际的月份1-12]
	 * @return {[number]}       []
	 */
	_datePtype.getMonthDaysCount = function() {
		var year = this.getFullYear();
		var month = this.getMonth();
		var monthDaysArr = [31, 28 + (isLeapYear(year) ? 1 : 0), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

		function isLeapYear(year) {
			return (year % 4 == 0) && (year % 100 != 0 || year % 400 == 0);
		}
		return monthDaysArr[month];
	}
	/**
	 * 获取6个星期*7的数据
	 * @return {[type]} [description]
	 */
	var extandCalendar = function() {
		_calPType.init = function() {
			var self = this,
				opts = self.options;
			self.setDate(opts.today);
			var $html = self.$El = $(calTmp),
				$headBar = $(HEADBAR).append(CURMONTH).append(MONTHSLE);

			if (opts.year) {
				$headBar.append(CURYEAR);
			}
			$html.append($headBar);
			$html.append(tableTmp);
			$html.find("thead").append(weekBarTmp);
			$html.delegate('.dp-table-wday,.dp-table-wwday,.dp-table-otherday', 'click', function(event) {
				var $el = $(this);
				var item = $el.html();
				if (item.trim() === "") {
					return;
				}
				var year = self._year;
				var month = self._month + 1;
				var date = year + "-" + (month < 10 ? "0" + month : month) + "-" + (item < 10 ? "0" + item : item);

				if (self.options.click2selected) {
					$el.toggleClass('active');
					var selected = self.options.selected;
					var index = selected.indexOf(date);
					var type = index == -1;
					type ? selected.push(date) : selected.splice(index, 1)
				}
				self.options.onClick && self.options.onClick(selected.slice(0, selected.length));
				
			});
			// $html.delegate('.dp-curmonth', 'click', function(event) {
			// 	var $el=$(this);
			// 	if(!$el.hasClass('active')){
			// 		$el.addClass('active');
			// 		setTimeout(function(){
			// 			$(document).one('click', function(event) {
			// 				/* Act on the event */
			// 				$el.removeClass('active');
			// 			});
			// 		},0);
			// 	}
			// });
			// $html.delegate('.dp-sel-item', 'click', function(event) {
			// 	var $el=$(this);
			// 	var month=MONTH.indexOf($el.html());
			// 	var diff=month-(self._today.getMonth());
			// 	console.log(self._today);
			// 	console.log(self.options.today);
			// 	// var start=getStartDate()
			// });
			self.setMonthHtml();
			opts.container.append($html);
		};
		_calPType.setMonthHtml = function(obj) {
			var self = this,
				monthDays = self.getMonthDays(self.options.otherdays && !self.options.multi),
				html = "<tr>",
				showOneTimes = 0, //判断是否为该月的日期
				week = 0,
				selected = self.options.selected,
				year = self._year,
				month = self._month + 1,
				$div = $("<div></div>");
			monthDays.forEach(function(item, i) {
				var w = i + 1;
				if (item == 1) {
					showOneTimes++
				};
				var cellHtml = cellTmp.replace("val", item);
				if (item === "&emsp;") {
					cellHtml = cellHtml.replace(cellClassName, "");
				} else {
					var curDate = year + "-" + (month < 10 ? "0" + month : month) + "-" + (item < 10 ? "0" + item : item);
					if (w % 7 === 0 || w % 7 === 6) {
						cellHtml = cellHtml.replace(cellClassName, wwClassName);
					}
					if (showOneTimes == 0 || showOneTimes == 2) {
						cellHtml = cellHtml.replace(cellClassName, otherClassName);
						cellHtml = cellHtml.replace(wwClassName, otherClassName);
					} else {
						var index=selected.indexOf(curDate); 
						if (index != -1) {
							if(obj){
								cellHtml = $div.html($(cellHtml).addClass('active').attr("title",obj[index].Name.trim()||"")).html();
							}
							cellHtml = $div.html($(cellHtml).addClass('active')).html();
						}
					}
					if (curDate === TODAY) {
						cellHtml = $div.html($(cellHtml).addClass('today')).html();
					}
				}
				html += cellHtml;
				var nowweek = w / 7
				if (w % 7 === 0 && week !== nowweek) {
					html += w === 42 ? "</tr>" : "</tr><tr>";
					week = nowweek;
				}
			});
			var $html = self.$El,
				opts = this.options;
			$html.find('tbody').html(html);
			$html.find('.dp-curmonth').html(MONTH[self._month]);
			if (opts.year) {
				$html.find('.dp-curyear').html(self._year);
			}
		};
		_calPType.getMonthDays = function(fillWithDate) {
			var year = this._year,
				month = this._month,
				monthDays = this._monthDays;
			var firDay = new Date(year, month, 1),
				lastDay = new Date(year, month, monthDays);
			var weekArr = getArr(monthDays);
			/*根据开始的星期补齐第一周的天数*/
			var firDayInWeek = firDay.getDay();
			firDayInWeek == 0 ? firDayInWeek = 7 : "";
			if (firDayInWeek != 1) {
				var diff = firDayInWeek - 1;
				var firWeekDay = new Date(firDay * 1 - diff * 8.64e7);
				var date = firWeekDay.getDate() * 1 + diff - 1; //自身算一天
				for (diff; diff > 0; diff--) {
					fillWithDate ? weekArr.unshift(date--) : weekArr.unshift("&emsp;");
				}
			}
			/*后面的天数就直接根据总长度来补齐*/
			var i = 1;
			while (weekArr.length < 42) {
				fillWithDate ? weekArr.push(i++) : weekArr.push("&emsp;");
			};
			return weekArr;
		};
		_calPType.setDate = function(today) {
			var self = this;
			if (!today) {
				today = new Date();
			} else {
				today = new Date(today);
			}
			self._today = today;
			self._year = today.getFullYear();
			self._month = today.getMonth();
			self._monthDays = today.getMonthDaysCount();
		};
		_calPType.nextMonth = function() {
			var self = this;
			self._today.setDate(self._monthDays);
			self.setDate(self._today * 1 + 8.64e7);
			self.setMonthHtml();
		};
		_calPType.preMonth = function() {
			var self = this;
			self._today.setDate(1);
			self.setDate(self._today * 1 - 8.64e7);
			self.setMonthHtml();
		};
		_calPType.nextYear = function() {
			var self = this;
			self.setDate(self._today.setYear(self._year * 1 + 1));
			self.setMonthHtml();
		};
		_calPType.preYear = function() {
			var self = this;
			self.setDate(self._today.setYear(self._year * 1 - 1));
			self.setMonthHtml();
		};
	}
	function turn(cals, type, yOrm) {
		if (yOrm) {
			cals.claArr.forEach(function(cal) {
				type == 1 ? cal.nextMonth() : cal.preMonth();
			})
		} else {
			cals.claArr.forEach(function(cal) {
				type == 1 ? cal.nextYear() : cal.preYear();
			})
		}
	}
	var Calendars = function(options, num) {
		var self = this;
		self.uid = Math.random() + "";
		var opts = self.options = $.extend({}, DEFAULTS, options);
		var cArr = self.claArr = [];
		var $cont = options.container;
		if (opts.turnyear) {
			$cont.append(ICON_TURN_YEAR);
		}
		var curMonth = opts.today.getMonth();
		//计算初始月份 当前月，数量 
		var preMonth = 0;
		var thisDay = opts.today.getFullYear() + "-01" + "-01";
		var startDate = getStartDate(thisDay, preMonth, true);
		if (!opts.wholeYear) {
			if (opts.turnmonth) {
				$cont.append(ICON_TURN_MONTH);
			}
			//preMonth = (num % 2 == 1 ? num + 1 : num) / 2 - 1;
			startDate = getStartDate(opts.today, preMonth, true);
		} else {
			num = 12;
		}
		$cont.delegate('.dp-pre,.dp-next', 'click', function(event) {
			var flag=isForbidden($(this).hasClass('month'));
			if(flag){
				self.options.onChangeYear&&self.options.onChangeYear($(".dp-curyear").eq(0).text());
				return;
			}
			turn(self, $(this).hasClass('dp-next'), $(this).hasClass('month'));
			self.options.onChangeYear&&self.options.onChangeYear($(".dp-curyear").eq(0).text());
		});
		for (var i = 0; i < num; i++) {
			var calOpts = $.extend({}, opts);
			calOpts.today = i == 0 ? startDate : getNextMonthDate(startDate);
			cArr.push(new Calendar(calOpts))
		};
	};
	Calendars.prototype.getSelected = function() {
		return this.options.selected;
	};
	Calendars.prototype.clearSelected = function() {
		this.options.selected.length = 0;
		this.claArr.forEach(function(cal) {
			cal.setMonthHtml();
		});
	};
	Calendars.prototype.setSelected = function(arr) {
		var selected = this.options.selected;
		selected.length = 0;
		arr.forEach(function(item) {
			selected.push(item)
		});
		this.claArr.forEach(function(cal) {
			cal.setMonthHtml();
		});
	};
	Calendars.prototype.reset = function() {
		console.log(this.today);
	};
	Calendars.prototype.setForbid=function(ids,flag){
		if(flag){
			$(ids).css("cssText","cursor:not-allowed!important");
		}else{
			$(ids).css("cssText","cursor:pointer!important");
		}
	}
	Calendars.prototype.setHoliday=function(obj){
		this.claArr.forEach(function(cal) {
			cal.setMonthHtml(obj);
		});
		
	}
	function isForbidden(yOrm){
		var cursor;
		if(yOrm){
			cursor = $(".month").css("cursor");	
		}else{
			cursor = $(".year").css("cursor");
		}
		if(cursor=="not-allowed"){
			return true;
		}else{
			return false;
		}
	}
	function getStartDate(curDate, MonthCount, pre) {
		var day = new Date(curDate);
		for (var i = 0; i < MonthCount; i++) {
			day = new Date(day.setDate(1) * 1 + (pre ? (-1) : 1) * 8.64e7);
		}
		return day;
	}
	function getNextMonthDate(date) {
		date.setDate(date.getMonthDaysCount());
		date.setTime(date * 1 + 8.64e7)
		return date;
	}
	function getArr(num) {
		var arr = []
		for (var i = 0; i < num;) {
			arr[i] = ++i;
		}
		return arr;
	}
	return function(options, num, selected) {
		num = num || 1;
		options = options || {};
		options.selected = selected || [];
		extandCalendar();
		var $cont = options.container = options.container ? $(options.container) : $(WRAPPER).appendTo("body");
		$cont.addClass('clearfix dp-wrapper');
		return new Calendars(options, num)
	}
})($);