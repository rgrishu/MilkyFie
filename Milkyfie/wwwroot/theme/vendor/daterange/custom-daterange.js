// Daterange
$(function () {
	var today = new Date();
	var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
	var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
	var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

	var dateTime = date + ' ' + time;

	$('#reportrange span').html(dateTime);
	//var start = moment().subtract(29, 'days');
	//var end = moment();
	//function cb(start, end) {
	//	$('#reportrange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
	//}
	//$('#reportrange').daterangepicker({
	//	opens: 'left',
	//	startDate: start,
	//	endDate: end,
	//	ranges: {
	//		'Today': [moment(), moment()],
	//		'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
	//		'Last 7 Days': [moment().subtract(6, 'days'), moment()],
	//		'Last 30 Days': [moment().subtract(29, 'days'), moment()],
	//		'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
	//	}
	//}, cb);
	//cb(start, end);
});