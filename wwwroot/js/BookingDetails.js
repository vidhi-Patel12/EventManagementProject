document.addEventListener('DOMContentLoaded', function (event) {
	Loaddetails()
});
function Loaddetails() {
	$.ajax({
		type: "POST",
		url: "/BookingDetail/LoadBookingdetails",
		data: {},
		success: function (Obj) {
			if (!Obj.isSuccess) {
				$('._CustomMessage').text(Obj.message);
				$('#successPopup').modal('show');
			}
			else {
				LoadAlldata(Obj.list);
				$('#divAddEditElements').hide();
			}
		},
		error: function () {

		}
	});
}

function LoadAlldata(List) {
	List.sort(function (a, b) {
		return b.bookingID - a.bookingID;
	});
	$('#tblbookingdetail').empty();
	$('#tblbookingdetail').dataTable({
		"pageLength": 10,
		"Processing": true,
		"destroy": true,
		"aaData": List,
		"order": [[0,"DESC"]],
		"columns": [

			{ "data": "bookingNo", "title": "BookingNo", "width": "70px" },
			{
				"data": "bookingDate", "title": "BookingDate", "width": "70px",
				"render": function (data, type, row) {
					if (data) {

						var bookingDate = new Date(data);

						var day = bookingDate.getDate();
						var monthIndex = bookingDate.getMonth();
						var year = bookingDate.getFullYear();

						var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

						return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
					} else {
						// If the data is null or undefined, return an empty string
						return "";
					}
				}

			},
			{
				"data": "bookingApproval", "title": "BookingApproval", "width": "70px",
				 "render": function (data) {
					if (data === "P") {
						return "Pending";
					} else if (data === "A") {
						return "Approved";
					} else if (data === "R") {
						return "Rejected";
					}
					else if (data === "C") {
						return "Cancelled";
					} else {
						// Code block to execute if none of the above conditions are true
					}
				}
			},
			{
				"data": "bookingApprovalDate", "title": "BookingApprovalDate", "width": "70px",
				"render": function (data, type, row) {
					if (data) {

						var bookingApprovalDate = new Date(data);

						var day = bookingApprovalDate.getDate();
						var monthIndex = bookingApprovalDate.getMonth();
						var year = bookingApprovalDate.getFullYear();

						var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

						return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
					} else {
						// If the data is null or undefined, return an empty string
						return "";
					}
				}
			},


			{
				"data": "bookingNo",
				"width": "70px",
				"title": "Process",
				"class": "text-center",
				"orderable": false,
				"render": function (data, bookingApproval, row) {
					if (row.bookingApproval === "P") {
				    	return `<button type="button" class="dt-btn-approve " style="color: green;" onclick="Viewdata('` + data.toString() + `')"> Process </button>`

					} else if (row.bookingApproval === "A") {
						return `<a type="text" > Approved </a>`

					} else if (row.bookingApproval === "C") {
						return `<a type="text" > Cancelled </a>`
					} else
						if (row.bookingApproval === "R") {
							return `<a type="text"> Rejected </a>`
						}
				}
			},

		]
	});
	$('#tblbookingdetail_wrapper').find(".row :first").prop("style", "margin-bottom:2%");
	$('#tblbookingdetail_wrapper').find("select[name='tblbookingdetail_length']").prop("style", "margin-top:5% ;width:35% !important;");
}

function Approve() {

	var DATA = [];
	DATA.push($("#BookingNo").val());
	DATA.push($("#BookingDate").val());
	//DATA.push($("#BookingID").attr('value'));
	//alert($("#BookingID").attr('value'));
	$.ajax({
		type: "POST",
		url: '/BookingDetail/BookingApproved',
		data: { "DATA": JSON.stringify(DATA) },
		success: function (data) {
			if (!data.isSuccess) {
				$('._CustomMessage').text(data.message);
				$('#errorPopup').modal('show');
			} else {

				$('._CustomMessage').text(data.message);
				$('#successPopup').modal('show');
				window.location.replace('/BookingDetail/BookingDetail');
			}
		},
		error: function () {
		}
	});

}

function REJECT() {
	var DATA = [];
	DATA.push($("#BookingNo").val());
	DATA.push($("#BookingDate").val());
	$.ajax({
		type: "POST",
		url: '/BookingDetail/BookingRejected',
		data: { "DATA": JSON.stringify(DATA) },
		success: function (data) {
			if (!data.isSuccess) {
				$('._CustomMessage').text(data.message);
				$('#errorPopup').modal('show');
			} else {

				$('._CustomMessage').text(data.message);
				$('#successPopup').modal('show');
				window.location.replace('/BookingDetail/BookingDetail');
			}
		},
		error: function () {
		}
	});
}

function Viewdata(Data) {
	$('#divBookingMaster').hide();
	$('#divAddEditElements').show();
	$.ajax({
		type: "GET",
		url: "/BookingDetail/Getdataforedit",
		data: { BookingNo: Data },


		success: function (data) {
			if (!data.isSuccess) {
				$('._CustomMessage').text(data.message);
			} else {
				$('._CustomMessage').text(data.message);
				if (data) {
					$('#BookingID').val(data.bookingID);

					$('#VenueCost').val(data.venueCost);
					$('#EquipmentCost').val(data.equipmentCost);
					$('#FoodCost').val(data.foodCost);
					$('#LightCost').val(data.lightCost);
					$('#FlowerCost').val(data.flowerCost);



					var venueCost = parseFloat(data.venueCost);
					var equipmentCosts = parseAndReplace(data.equipmentCost);
					var foodCosts = parseAndReplace(data.foodCost);
					var lightCost = parseAndReplace(data.lightCost);
					var flowerCost = parseAndReplace(data.flowerCost);

					var totalAmount = venueCost + equipmentCosts + foodCosts + lightCost + flowerCost;


					if (typeof totalAmount === 'number' && !isNaN(totalAmount)) {
						$('#TotalAmount').val(totalAmount.toFixed(2));
					} else {
						console.error(error);
					}
					//alert($('#TotalAmount').val(totalAmount.toFixed(2)));
					$('#BookingNo').val(data.bookingNo);
					$('#BookingDate').val(data.bookingDate);



					//$('#divAddEditElements').hide();
				}
			}
		},
		error: function () {
			alert("An error occurred while updating details.");
		}
	});
}
function parseAndReplace(value) {
	debugger;

	if (Array.isArray(value)) {
		return value.reduce((sum, part) => sum + parseFloat(part), 0);
	} else if (typeof value === 'number') {
		return value;
	} else {
		return value;
	}
}
