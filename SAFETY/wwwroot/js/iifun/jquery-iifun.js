$(function() {
	
	// 設定sidebarMenu viewport高度
	function sidebarMenu(){
		$('#sidebarMenu .viewport').css('height', $(window).innerHeight());
	}
	$(window).on("resize", sidebarMenu);
	sidebarMenu();

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
    $('.ListItemTable').each(function () {
        $('a.open_collapse_cont').click(function () {
            $(this).parents('.item').toggleClass('show').removeClass('show_attachment');
        });
        $('.edit_btns .edit').click(function () {
            $(this).parents('.item').toggleClass('show').removeClass('show_attachment');
        });
        $('a.open_attachment').click(function () {
            $(this).parents('.item').toggleClass('show_attachment').removeClass('show');
        });
        $('a.open_check').click(function () {
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
    $('.show_table_btn').click(function () {
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
    
});