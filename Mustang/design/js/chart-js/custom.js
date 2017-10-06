var ctx = document.getElementById("barChart").getContext('2d');
var barChart = new Chart(ctx, {
	type: 'bar',
	data: {
		labels: ["M", "T", "W", "T", "F", "S", "S"],
		datasets: [{
			label: 'Male',
			data: [12, 19, 3, 17, 28, 24, 7],
			backgroundColor: "#3a86c8"
		}, {
			label: 'Female',
			data: [30, 29, 5, 5, 20, 3, 10],
			backgroundColor: "#ea423f"
		}]
	},
	options: {
		maintainAspectRatio: false,
	},
});


var ctx = document.getElementById('areaChart').getContext('2d');
var areaChart = new Chart(ctx, {
	type: 'line',
	data: {
		labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
		datasets: [{
			label: 'Sales',
			data: [12, 19, 3, 17, 6, 3, 7],
			backgroundColor: "rgb(255, 99, 132)"
		}, {
			label: 'Expenses',
			data: [2, 29, 5, 5, 2, 3, 10],
			backgroundColor: "rgb(231, 233, 237)"
		}]
	},
	options: {
		maintainAspectRatio: false,
	},	
});


var ctx = document.getElementById("radarChart");
var radarChart = new Chart(ctx, {
	type: 'radar',
	data: {
		labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
		datasets: [{
			label: 'Likes',
			backgroundColor: "rgba(54, 162, 235, 0.8)",
			borderColor: "rgba(255, 205, 86, 0.8)",
			data: [12, 19, 3, 17, 28, 24, 7],
			lineTension: 0.1,
			borderColor: "#fff",
		}, {
			label: 'Shares',
			backgroundColor: "rgba(255, 99, 132, 0.8)",
			borderColor: "rgba(153, 102, 255, 0.8)",
			data: [30, 29, 5, 5, 20, 3, 10],
			lineTension: 0.1,
			borderColor: "#fff",
		}]
	},
	options: {
		maintainAspectRatio: false,
	},	
});



var ctx = document.getElementById("polarChart").getContext('2d');
var polarChart = new Chart(ctx, {
	type: 'polarArea',
	data: {
		labels: ["Jan", "Feb", "Mar", "Apr"],
		datasets: [{
			label: "Polar Chart",
			fill: false,
			lineTension: 0.1,
			borderColor: "#fff",
			pointBackgroundColor: "#fff",
			pointBorderWidth: 1,
			pointHoverRadius: 5,
			pointHoverBackgroundColor: "#fff",
			pointHoverBorderColor: "rgba(220, 220, 220, 1)",
			pointHoverBorderWidth: 2,
			pointRadius: 1,
			pointHitRadius: 10,
			data: [88, 37, 69, 121],
			spanGaps: false,
			backgroundColor: [
				"rgb(255, 99, 132)",
				"rgb(255, 159, 64)",
				"rgb(255, 205, 86)",
				"rgb(75, 192, 192)",
				"rgb(54, 162, 235)",
				"rgb(153, 102, 255)",
				"rgb(231,233,237)",
			],
		}]
	},
	options: {
		responsive: true,
		legend: {
			position: 'top',
		},
		title: {
			display: true,
			text: 'Polar Chart'
		}
	},
	options: {
		maintainAspectRatio: false,
	},	
});


var ctx = document.getElementById("pieChart").getContext('2d');
var pieChart = new Chart(ctx, {
	type: 'pie',
	data: {
		labels: ["Q1", "Q2", "Q3", "Q4"],
		datasets: [{
			label: "Pie Chart",
			fill: false,
			lineTension: 0.1,
			borderColor: "#fff",
			pointBackgroundColor: "#fff",
			pointBorderWidth: 1,
			pointHoverRadius: 5,
			pointHoverBackgroundColor: "#fff",
			pointHoverBorderColor: "rgba(220, 220, 220, 1)",
			pointHoverBorderWidth: 2,
			pointRadius: 1,
			pointHitRadius: 10,
			data: [88, 37, 69, 121],
			spanGaps: false,
			backgroundColor: [
				"rgb(255, 99, 132)",
				"rgb(255, 159, 64)",
				"rgb(255, 205, 86)",
				"rgb(75, 192, 192)",
				"rgb(54, 162, 235)",
				"rgb(153, 102, 255)",
				"rgb(231,233,237)",
			],
		}]
	},
	options: {
		responsive: true,
		maintainAspectRatio: false,
		legend: {
			position: 'top',
		},
		title: {
			display: true,
			text: 'Pie Chart'
		}
	}
});


var ctx = document.getElementById("doughnutChart").getContext('2d');
var doughnutChart = new Chart(ctx, {
	type: 'doughnut',
	data: {
		labels: ["M", "T", "W", "T", "F", "S", "S"],
		datasets: [{
			label: "Doughnut Chart",
			fill: false,
			lineTension: 0.1,
			borderColor: "#fff",
			pointBackgroundColor: "#fff",
			pointBorderWidth: 1,
			pointHoverRadius: 5,
			pointHoverBackgroundColor: "#fff",
			pointHoverBorderColor: "rgba(220,220,220,1)",
			pointHoverBorderWidth: 2,
			pointRadius: 1,
			pointHitRadius: 10,
			data: [65, 59, 80, 81, 56, 55, 40],
			spanGaps: false,
			backgroundColor: [
				"rgb(255, 99, 132)",
				"rgb(255, 159, 64)",
				"rgb(255, 205, 86)",
				"rgb(75, 192, 192)",
				"rgb(54, 162, 235)",
				"rgb(153, 102, 255)",
				"rgb(231,233,237)",
			],
		}]
	},
	options: {
		maintainAspectRatio: false,
		responsive: true,
		legend: {
			position: 'top',
		},
		title: {
			display: true,
			text: 'Doughnut Chart'
		}
	}
});
