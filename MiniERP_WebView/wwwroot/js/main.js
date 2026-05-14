/* global Chart, coreui */

/**
 * --------------------------------------------------------------------------
 * CoreUI Boostrap Admin Template main.js
 * Licensed under MIT (https://github.com/coreui/coreui-free-bootstrap-admin-template/blob/main/LICENSE)
 * --------------------------------------------------------------------------
 */

// Configure Chart.js defaults
if (typeof Chart !== 'undefined' && typeof coreui !== 'undefined' && coreui.ChartJS) {
  Chart.defaults.pointHitDetectionRadius = 1;
  Chart.defaults.plugins.tooltip.enabled = false;
  Chart.defaults.plugins.tooltip.mode = 'index';
  Chart.defaults.plugins.tooltip.position = 'nearest';
  Chart.defaults.plugins.tooltip.external = coreui.ChartJS.customTooltips;
  
  try {
    Chart.defaults.defaultFontColor = coreui.Utils.getStyle('--cui-body-color');
  } catch (e) { }

  document.documentElement.addEventListener('ColorSchemeChange', () => {
    // Correct null check: if (var) instead of typeof
    if (window.cardChart1) {
      window.cardChart1.data.datasets[0].pointBackgroundColor = coreui.Utils.getStyle('--cui-primary');
      window.cardChart1.update();
    }
    if (window.cardChart2) {
      window.cardChart2.data.datasets[0].pointBackgroundColor = coreui.Utils.getStyle('--cui-info');
      window.cardChart2.update();
    }
    if (window.mainChart) {
      window.mainChart.options.scales.x.grid.color = coreui.Utils.getStyle('--cui-border-color-translucent');
      window.mainChart.options.scales.x.ticks.color = coreui.Utils.getStyle('--cui-body-color');
      window.mainChart.options.scales.y.border.color = coreui.Utils.getStyle('--cui-border-color-translucent');
      window.mainChart.options.scales.y.grid.color = coreui.Utils.getStyle('--cui-border-color-translucent');
      window.mainChart.options.scales.y.ticks.color = coreui.Utils.getStyle('--cui-body-color');
      window.mainChart.update();
    }
  });
}

const random = (min, max) => Math.floor(Math.random() * (max - min + 1) + min);

// Initialize charts ONLY if elements exist and attach to window for global access
const chart1El = document.getElementById('card-chart1');
window.cardChart1 = chart1El ? new Chart(chart1El, {
  type: 'line',
  data: {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [{
      label: 'My First dataset',
      backgroundColor: 'transparent',
      borderColor: 'rgba(255,255,255,.55)',
      pointBackgroundColor: (typeof coreui !== 'undefined') ? coreui.Utils.getStyle('--cui-primary') : '#fff',
      data: [65, 59, 84, 84, 51, 55, 40]
    }]
  },
  options: {
    plugins: { legend: { display: false } },
    maintainAspectRatio: false,
    scales: {
      x: { border: { display: false }, grid: { display: false, drawBorder: false }, ticks: { display: false } },
      y: { min: 30, max: 89, display: false, grid: { display: false }, ticks: { display: false } }
    },
    elements: { line: { borderWidth: 1, tension: 0.4 }, point: { radius: 4, hitRadius: 10, hoverRadius: 4 } }
  }
}) : null;

const chart2El = document.getElementById('card-chart2');
window.cardChart2 = chart2El ? new Chart(chart2El, {
  type: 'line',
  data: {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [{
      label: 'My First dataset',
      backgroundColor: 'transparent',
      borderColor: 'rgba(255,255,255,.55)',
      pointBackgroundColor: (typeof coreui !== 'undefined') ? coreui.Utils.getStyle('--cui-info') : '#fff',
      data: [1, 18, 9, 17, 34, 22, 11]
    }]
  },
  options: {
    plugins: { legend: { display: false } },
    maintainAspectRatio: false,
    scales: {
      x: { border: { display: false }, grid: { display: false, drawBorder: false }, ticks: { display: false } },
      y: { min: -9, max: 39, display: false, grid: { display: false }, ticks: { display: false } }
    },
    elements: { line: { borderWidth: 1 }, point: { radius: 4, hitRadius: 10, hoverRadius: 4 } }
  }
}) : null;

const mainChartEl = document.getElementById('main-chart');
window.mainChart = mainChartEl ? new Chart(mainChartEl, {
  type: 'line',
  data: {
    labels: ['October', 'November', 'December', 'January', 'February', 'March', 'April'],
    datasets: [{
      label: 'My First dataset',
      backgroundColor: (typeof coreui !== 'undefined') ? `rgba(${coreui.Utils.getStyle('--cui-info-rgb')}, .1)` : 'rgba(0,0,0,0.1)',
      borderColor: (typeof coreui !== 'undefined') ? coreui.Utils.getStyle('--cui-info') : '#000',
      pointHoverBackgroundColor: '#fff',
      borderWidth: 2,
      data: [random(50, 200), random(50, 200), random(50, 200), random(50, 200), random(50, 200), random(50, 200), random(50, 200)],
      fill: true
    }]
  },
  options: {
    maintainAspectRatio: false,
    plugins: { legend: { display: false } },
    scales: {
      x: { grid: { drawOnChartArea: false } },
      y: { beginAtZero: true, maxTicksLimit: 5 }
    },
    elements: { line: { tension: 0.4 }, point: { radius: 0, hitRadius: 10 } }
  }
}) : null;