// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(() => {
  // Toggle opening and closing of sidebar
  const sidebar = document.querySelector("#sidebar");

  document.querySelector("#open-links").addEventListener("click", () => {
    sidebar.classList.remove("sidebar--hidden");
  });

  document.querySelector("#close-links").addEventListener("click", () => {
    sidebar.classList.add("sidebar--hidden");
  });

  const links = document.querySelectorAll("[data-linkcontent]");
  const qrVisibleClass = "qr-wrapper--visible";
  const hiddenClass = "btn-text__item--hidden";

  links.forEach((link) => {
    let copyBtnRecentlyClicked = false;

    const copyBtn = link.querySelector("[copy-btn]");
    const qrBtn = link.querySelector("[qr-btn]");
    const qrWrapper = link.querySelector("[qr-wrapper]");
    const qrWrapperInternal = link.querySelector("[qr-wrapper-internal]");
    const qrWrapperBtn = link.querySelector("[qr-wrapper-btn]");
    const qrWrapperCloseBtn = link.querySelector("[qr-wrapper-btn-close]");

    qrWrapperInternal.innerHTML = "";
    new QRCode(qrWrapperInternal, {
      text: link.dataset.linkcontent,
      width: 128,
      height: 128,
    });

    qrBtn.addEventListener("click", async () => {
      qrWrapper.classList.add(qrVisibleClass);
    });

    qrWrapperBtn.addEventListener("click", () => {
      const canvas = link.querySelector("canvas");
      const dataURL = canvas.toDataURL("image/png");

      const downloadLink = document.createElement("a");
      downloadLink.href = dataURL;
      downloadLink.download = "QR-Code.png";

      downloadLink.click();
      downloadLink.remove();
    });

    qrWrapperCloseBtn.addEventListener("click", () => {
      qrWrapper.classList.remove(qrVisibleClass);
    });

    const mainText = link.querySelector("[copy-primary]");
    const secondaryText = link.querySelector("[copy-secondary]");

    copyBtn.addEventListener("click", async () => {
      try {
        await navigator.clipboard.writeText(link.dataset.linkcontent);

        if (!copyBtnRecentlyClicked) {
          copyBtnRecentlyClicked = true;

          secondaryText.classList.remove(hiddenClass);
          mainText.classList.add(hiddenClass);

          setTimeout(() => {
            copyBtnRecentlyClicked = false;

            mainText.classList.remove(hiddenClass);
            secondaryText.classList.add(hiddenClass);
          }, 1500);
        }
      } catch {
        console.error("Could not copy link to clipboard.");
      }
    });
  });

  // Hamburger menu
  const hamburger = document.querySelector("#hamburger");
  const navList = document.querySelector("#nav-list");
  const visibleClass = "nav__list--visible";
  let isOpen = false;

  hamburger.addEventListener("click", () => {
    isOpen = !isOpen;
    // Toggle wasn't working for some reason?
    if (isOpen) {
      navList.classList.add(visibleClass);
    } else {
      navList.classList.remove(visibleClass);
    }
  });
})();
