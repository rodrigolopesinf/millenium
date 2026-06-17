/* -------------------------------------------------------------
   MILLENIUM PESQUISAS - SCRIPT INSTITUCIONAL
   ------------------------------------------------------------- */

document.addEventListener("DOMContentLoaded", function () {
    
    // 1. Menu Mobile Toggle
    const menuToggle = document.getElementById("menuToggle");
    const navMenu = document.getElementById("navMenu");

    if (menuToggle && navMenu) {
        menuToggle.addEventListener("click", function () {
            const isActive = navMenu.classList.contains("active");
            navMenu.classList.toggle("active");
            menuToggle.setAttribute("aria-expanded", !isActive);
            
            // Animação das barras do hambúrguer
            const bars = menuToggle.querySelectorAll(".bar");
            if (!isActive) {
                bars[0].style.transform = "rotate(45deg) translate(6px, 6px)";
                bars[1].style.opacity = "0";
                bars[2].style.transform = "rotate(-45deg) translate(5px, -5px)";
            } else {
                bars[0].style.transform = "none";
                bars[1].style.opacity = "1";
                bars[2].style.transform = "none";
            }
        });

        // Fechar o menu ao clicar em links
        const navLinks = navMenu.querySelectorAll(".nav-link, .btn-login-header");
        navLinks.forEach(link => {
            link.addEventListener("click", () => {
                navMenu.classList.remove("active");
                menuToggle.setAttribute("aria-expanded", "false");
                const bars = menuToggle.querySelectorAll(".bar");
                bars[0].style.transform = "none";
                bars[1].style.opacity = "1";
                bars[2].style.transform = "none";
            });
        });
    }

    // 2. Navegação de Abas (Tabs) dos Serviços
    const tabButtons = document.querySelectorAll(".tab-btn");
    const tabContents = document.querySelectorAll(".tab-content");

    if (tabButtons.length > 0 && tabContents.length > 0) {
        tabButtons.forEach(btn => {
            btn.addEventListener("click", function () {
                const targetTab = this.getAttribute("data-tab");

                // Remove classe ativa de todos os botões e conteúdos
                tabButtons.forEach(b => b.classList.remove("active"));
                tabContents.forEach(c => c.classList.remove("active"));

                // Adiciona classe ativa no botão clicado e no respectivo conteúdo
                this.classList.add("active");
                const activeContent = document.getElementById(targetTab);
                if (activeContent) {
                    activeContent.classList.add("active");
                }
            });
        });
    }

    // 3. Validação básica do formulário de contato no submit
    const contactForm = document.querySelector(".contact-form-panel form");
    if (contactForm) {
        contactForm.addEventListener("submit", function (e) {
            const nome = document.getElementById("nome").value.trim();
            const email = document.getElementById("email").value.trim();
            const assunto = document.getElementById("assunto").value.trim();
            const mensagem = document.getElementById("mensagem").value.trim();

            if (!nome || !email || !assunto || !mensagem) {
                e.preventDefault();
                alert("Por favor, preencha todos os campos do formulário de contato.");
            }
        });
    }
});
