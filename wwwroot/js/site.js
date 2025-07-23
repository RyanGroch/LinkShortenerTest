// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

console.log("???");

const sidebar = document.querySelector("#sidebar");

document.querySelector("#open-links").addEventListener("click", () => {
  sidebar.classList.remove("sidebar--hidden");
});

document.querySelector("#close-links").addEventListener("click", () => {
  sidebar.classList.add("sidebar--hidden");
});
