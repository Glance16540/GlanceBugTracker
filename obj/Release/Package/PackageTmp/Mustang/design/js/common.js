'use strict';

$(function () {

	// ScrollUp
	$.scrollUp({
		scrollName: 'scrollUp',
		scrollDistance: 180,
		scrollFrom: 'top',
		scrollSpeed: 300,
		easingType: 'linear',
		animation: 'fade',
		animationSpeed: 200,
		scrollTrigger: false,
		scrollText: '<i class="icon-flight"></i>',
		scrollTitle: false,
		scrollImg: false,
		activeOverlay: false,
	});

	// Header todo list dropdown
	$('.dropdown-menu .todo').on('click', function(e) {
		e.stopPropagation();
	});

	// Task list
	$('.task-list').on('click', 'li.list', function() {
		$(this).toggleClass('completed');
	});

	// Page loading
	setTimeout(function() {
		$('body').addClass('loaded');
	}, 3000);

	// Tooltip & Popover
	$('[data-toggle="tooltip"]').tooltip();
	$('[data-toggle="popover"]').popover();

	// Theming
	$('#themeOptions').styleSwitcher();

});