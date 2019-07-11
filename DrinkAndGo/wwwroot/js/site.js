﻿// Write your JavaScript code.

d3.select(".chart")
    .selectAll("div")
    .data(data)
    .enter().append("div")
    .style("width", function (d) { return d * 10 + "px"; })
    .text(function (d) { return d; });




function eqfeed_callback(response) {
  map.data.addGeoJson(response);
}