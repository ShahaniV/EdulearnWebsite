
$('.list-header').on('click', function () {
    var $J_li = $(this).parents('.J_list')
    $J_li.hasClass('open') ? $J_li.removeClass('open') : $J_li.addClass('open');
})

$(function () {

    $('#kmodules').coverflow({
      active: 2,
      select: function(event, ui) {
        console.log();
      }
    });
  
    $('.ui-state-active a').click(function() {
      window.location = $this.attr('href');
    });
  
    $('#coverflow-kmodules .fa-chevron-right').click(function() {
      $('#kmodules').coverflow('next');
    });
  
    $('#coverflow-kmodules .fa-chevron-left').click(function() {
      $('#kmodules').coverflow('prev');
    });
  
    $("body").keydown(function(e) {
      // left arrow
      if ((e.keyCode || e.which) == 37) {
        $('#kmodules').coverflow('prev');
      }
      // right arrow
      if ((e.keyCode || e.which) == 39) {
        $('#kmodules').coverflow('next');
      }
    });
  
  });