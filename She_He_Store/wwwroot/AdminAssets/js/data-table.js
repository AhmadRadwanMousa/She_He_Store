//(function($) {
//  'use strict';
//  $(function() {
//      $('#exampleTable').DataTable({
//      "aLengthMenu": [
//        [5, 10, 15, -1],
//        [5, 10, 15, "All"]
//      ],
//      "iDisplayLength": 10,
//      "language": {
//        search: ""
//      }
//    });
//    $('#order-listing').each(function() {
//      var datatable = $(this);
//      // SEARCH - Add the placeholder for Search and Turn this into in-line form control
//      //var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
//      //search_input.attr('placeholder', 'Search');
//      //search_input.removeClass('form-control-sm');
//      // LENGTH - Inline-Form control
//      var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_length] select');
//      length_sel.removeClass('form-control-sm');
//    });
//  });
//})(jQuery);

$(document).ready(function () {
    var table = $('#productTable').DataTable({
        "paging": true, // Enable pagination
        "lengthChange": false, // Disable page length options
        "searching": true, // Enable search
        "ordering": true, // Enable column sorting
        "info": true, // Show table information
        "autoWidth": false, // Disable auto-width
        "dom": 'Bfrtip', // Add export buttons
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
  
});
$(document).ready(function () {
    var table = $('#orderTable').DataTable({
        "paging": true, // Enable pagination
        "lengthChange": false, // Disable page length options
        "searching": true, // Enable search
        "ordering": true, // Enable column sorting
        "info": true, // Show table information
        "autoWidth": false, // Disable auto-width
        "dom": 'Bfrtip', // Add export buttons
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
 
});
