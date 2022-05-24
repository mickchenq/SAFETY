$(function() {
	
	// 設定sidebarMenu viewport高度
	function sidebarMenu(){
		$('#sidebarMenu .viewport').css('height', $(window).innerHeight());
	}
	$(window).on("resize", sidebarMenu);
	sidebarMenu();
	
	// sidebarMenu第二層選單開合
	$('#sidebarMenu ul.list > li > a').click(function(){
		// 當點到標題時，若答案是隱藏時則顯示它，同時隱藏其它已經展開的項目
		// 反之則隱藏
		var $qa_content = $(this).next('ul'),
			$scrollbar = $('#sidebarMenu > .scrollwrap').data("plugin_tinyscrollbar");

		if(!$qa_content.is(':visible')){
			$('#sidebarMenu ul.list > li > a').next('ul').slideUp();
			$(this).parent("li").addClass('slideDown').siblings().removeClass('slideDown');
		}
		else {
			$(this).parent("li").removeClass('slideDown');
		}
		$qa_content.slideToggle(
			//展開收合時更新tinyscrollbar高度
			function()
			{
				$scrollbar.update("resize");
			}
		);
	}).siblings('ul').hide();
	//若下面還有子選單，加上class=arrow
	if($("#sidebarMenu ul").length > 0) {
		$("#sidebarMenu ul").parent("li").find("> a").addClass("arrow");
	}

	// sidebarMenu開合
	$('.menu-options').click(function(){
		var _text = $(this).find('span:first-child')
		if($(this).is('.active')) {
			$('#sidebarMenu').addClass('menu_slide');
			$('#Wrap').addClass('wide');
			$(this).removeClass('active');
			//_text.text('開啟選單');
			_text.text(localizer_openmenu);
		}
		else {
			$('#sidebarMenu').removeClass('menu_slide');
			$('#Wrap').removeClass('wide');
			$(this).addClass('active');
			//_text.text('關閉選單');
			_text.text(localizer_closemenu);
		}
	});
	
	// 浮動主選單
	$(window).bind('scroll resize', function(){
		var scrollVal = $(this).scrollTop();
		
			if(scrollVal > 149) {
				$('#Wrap').addClass('MenuFix')
			}
			else {
				$('#Wrap').removeClass('MenuFix')
			}
		}).scroll();	// 觸發一次 scroll()
	
	// 文字列表
	$('.rwdTable td').each(function(){
		var _thText = $(this).parents('.rwdTable').find('th').eq($(this).index()).text();
		$(this).attr("data-th", _thText);
	});
	
	// 通用文字列表
	$('.ListItemTable').each(function(){
		$('a.open_collapse_cont').click(function() {
			$(this).parents('.item').toggleClass('show').removeClass('show_attachment');
		});
		$('.edit_btns .edit').click(function() {
			$(this).parents('.item').toggleClass('show').removeClass('show_attachment');
		});
		$('a.open_attachment').click(function() {
			$(this).parents('.item').toggleClass('show_attachment').removeClass('show');
		});
		$('a.open_check').click(function() {
			$(this).parents('.item').toggleClass('show');
		});
	});
	
	// 產品列表
	$('.ProItemList').each(function(){
		$('a.ProTitle').click(function() {
			$(this).parents('.ProItem').toggleClass('show');
		});
	});
	
	// 產品列表
	$('.slideTitle .text').click(function() {
		$(this).parents('.slideTitle').toggleClass('show');
		$(this).parents('.slideTitle').next('.slideCont').toggleClass('show');
		$(this).parents('.slideTitle').next('.slideCont').slideToggle();
	});
	
	// 列表收合採購單號的品項
    $('.table_toggle_btn').click(function() {
        $(this).toggleClass('show');
        $(this).next('.sub_table').toggleClass('show');
    });
	
	// 列表收合請購申請簽核的簽核歷程
    $('.show_table_btn').click(function() {
        $(this).toggleClass('show');
        $('.ListTable').toggleClass('hide').toggleClass('show');
    });
    
    //popup box
	var _popupBox = $('.popup_box')
	
	_popupBox.find('.close').click(function() {
		$(this).parents('.popup_box').removeClass('show')
		$('body').removeClass('fixed')
	});
	
	_popupBox.find('.cancel').click(function() {
		$(this).parents('.popup_box').removeClass('show')
		$('body').removeClass('fixed')
	});
    
    // 行事曆
	$('.Calendar table.grid .event .event_title').click(function(){
		$(this).next('.event_cont_popup').addClass('show');
		$(this).parents('.month_row').siblings().find('table.grid .event .event_cont_popup').removeClass('show');
		$(this).parents('.event').siblings().find('.event_cont_popup').removeClass('show');
	});
	$('.CalendarMobile span.event').click(function(){
		$(this).next('.event_cont_popup').addClass('show').siblings().removeClass('show');
		$(this).parent('li').siblings().find('.event_cont_popup').removeClass('show');
	});
	$('.CalendarWrap .event_cont_popup .close').click(function(){
		$(this).parent('.event_cont_popup').removeClass('show');
	});
    
});