$('#defaultBar').barIndicator();

// Bar Color
var opt = {foreColor:'#e84f4c'};$('#barColor').barIndicator(opt);

// Bar Height
var opt = { horBarHeight:27}; $('#barHeight').barIndicator(opt);

// Vertical Height
var opt = { style:'vertical'}; $('#barVertical').barIndicator(opt);
var opt = { style:'vertical', foreColor:'#e84f4c'}; $('#barVertical2').barIndicator(opt);
var opt = { style:'vertical', foreColor:'#71c586'}; $('#barVertical3').barIndicator(opt);

// Bar Holder Color
var opt = {foreColor:'#ffb400', backColor:'#3693cf'}; $('#barHolderColor').barIndicator(opt);

// label Positions
var opt = {horLabelPos:'topRight', foreColor:'#e84f4c'};$('#barLabelTopRight').barIndicator(opt);
var opt = {horLabelPos:'right', foreColor:'#3693cf'};$('#barLabelRight').barIndicator(opt);
var opt = {horLabelPos:'left', foreColor:'#71c586'};$('#barLabelLeft').barIndicator(opt);