---
name: ui-design-system
description: Diretrizes e padrões para criação de interfaces profissionais e elegantes em HTML com Blazor.
---

# Sistema de Design de UI para Blazor

Este documento serve como a especificação de estilo, estrutura e melhores práticas para a criação de componentes e telas em **Blazor (HTML/Razor + CSS + C#)**. Siga estas diretrizes para garantir interfaces refinadas, modernas, fluidas e acessíveis.

---

## 1. Princípios de Design e Estética Premium

Para que as interfaces pareçam sofisticadas e profissionais, siga estas regras visuais:

*   **Tipografia Moderna:** Evite as fontes padrão do navegador. Utilize fontes modernas do Google Fonts como *Inter*, *Outfit*, *Plus Jakarta Sans* ou *Cabinet Grotesk*.
*   **Paleta de Cores Curada:** Nunca utilize cores puras (como `#FF0000` para vermelho ou `#0000FF` para azul). Prefira paletas HSL equilibradas, tons pastéis para fundos e tons escuros/profundos para textos e contrastes.
*   **Modo Escuro (Dark Mode):** Desenvolva com suporte nativo a temas ou priorize um tema escuro elegante (utilizando cores como `#0B0F19` ou `#121827` em vez de preto puro `#000`).
*   **Efeitos de Vidro (Glassmorphism):** Para painéis flutuantes, modais e barras de navegação superiores, utilize `backdrop-filter: blur(12px)` combinado com fundos semi-transparentes (`rgba(..., 0.7)`) e bordas sutis de 1px.
*   **Micro-interações e Animações:** Cada botão, link ou card deve responder ao cursor com transições suaves (`transition: all 0.2s ease-in-out`). Use animações de entrada discretas (`fade-in`, `slide-up`) para carregamento de componentes.
*   **Espaçamento Generoso:** Dê "respiro" aos elementos. Utilize margens e paddings amplos para separar seções logicamente.

---

## 2. Estrutura de Componentes Blazor

No Blazor, a componentização limpa e a separação de responsabilidades são essenciais.

### 2.1 Padrão de Arquivos
Sempre que possível, utilize **CSS Escopado** para evitar colisão de estilos globais:
*   `MeuComponente.razor` (Estrutura HTML/Razor + Parâmetros)
*   `MeuComponente.razor.css` (Estilos específicos do componente)
*   `MeuComponente.razor.cs` (Código C# / Lógica - Code-Behind, opcional para componentes complexos)

### 2.2 Exemplo de Componente Elegante (Card Informativo)

#### [MeuCard.razor]
```html
<div class="premium-card @(Active ? "active" : "")" @onclick="HandleClick">
    <div class="card-glow"></div>
    <div class="card-icon">
        @IconTemplate
    </div>
    <div class="card-content">
        <h3 class="card-title">@Title</h3>
        <p class="card-description">@Description</p>
    </div>
    <div class="card-badge">@BadgeText</div>
</div>
```

#### [MeuCard.razor.css]
```css
.premium-card {
    position: relative;
    display: flex;
    flex-direction: column;
    padding: 1.5rem;
    border-radius: 16px;
    background: rgba(255, 255, 255, 0.03);
    border: 1px solid rgba(255, 255, 255, 0.08);
    backdrop-filter: blur(8px);
    overflow: hidden;
    cursor: pointer;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.premium-card:hover {
    transform: translateY(-4px);
    border-color: var(--color-primary-light, #6366f1);
    box-shadow: 0 12px 30px rgba(0, 0, 0, 0.2);
}

.premium-card.active {
    background: rgba(99, 102, 241, 0.1);
    border-color: #6366f1;
}

.card-glow {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: radial-gradient(circle at top right, rgba(99, 102, 241, 0.15), transparent 60%);
    pointer-events: none;
}

.card-title {
    font-size: 1.25rem;
    font-weight: 600;
    color: #ffffff;
    margin: 0 0 0.5rem 0;
    font-family: 'Outfit', sans-serif;
}

.card-description {
    font-size: 0.925rem;
    color: #9ca3af;
    line-height: 1.5;
    margin: 0;
}

.card-badge {
    align-self: flex-start;
    margin-top: 1rem;
    padding: 0.25rem 0.75rem;
    border-radius: 9999px;
    font-size: 0.75rem;
    font-weight: 600;
    background: rgba(99, 102, 241, 0.2);
    color: #a5b4fc;
    border: 1px solid rgba(99, 102, 241, 0.3);
}
```

#### [MeuCard.razor.cs]
```csharp
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Clarium.Agent.Skills.UiDesignSystem;

public partial class MeuCard : ComponentBase
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Description { get; set; } = string.Empty;
    [Parameter] public string BadgeText { get; set; } = string.Empty;
    [Parameter] public RenderFragment? IconTemplate { get; set; }
    [Parameter] public bool Active { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private async Task HandleClick(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }
}
```

---

## 3. Diretrizes de CSS e Variáveis Globais

Centralize a identidade visual em variáveis de CSS no arquivo de estilos globais (`app.css` ou `site.css`):

```css
:root {
    /* Cores de Fundo e Texto */
    --bg-primary: #0b0f19;
    --bg-secondary: #111827;
    --bg-tertiary: #1f2937;
    --text-primary: #f9fafb;
    --text-secondary: #9ca3af;
    --text-muted: #6b7280;

    /* Cores de Destaque (Accent) */
    --color-primary: #6366f1;
    --color-primary-light: #818cf8;
    --color-primary-glow: rgba(99, 102, 241, 0.15);
    --color-success: #10b981;
    --color-danger: #ef4444;

    /* Bordas e Sombras */
    --radius-sm: 8px;
    --radius-md: 12px;
    --radius-lg: 16px;
    --border-color: rgba(255, 255, 255, 0.08);
    --shadow-premium: 0 10px 30px -10px rgba(0, 0, 0, 0.5);
}
```

---

## 4. Estado de Carregamento (Loading States) e Esqueletos

Para manter a percepção de velocidade da aplicação Blazor (especialmente se for Blazor Server ou realizar chamadas assíncronas):
1.  **Skeleton Screens:** Use esqueletos pulsantes em vez de spinners genéricos para carregar listas ou tabelas.
2.  **Transições de Estado:** Utilize animações de esmaecimento ao alternar visibilidades ou renderizar componentes condicionalmente (`@if (loading)`).

```css
@keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: .5; }
}
.skeleton {
    animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
    background-color: var(--bg-tertiary);
    border-radius: 4px;
}
```

---

## 5. Responsividade e Layout

*   **Mobile-First:** Sempre comece construindo o design focado em mobile, expandindo para telas maiores com `@media (min-width: 768px)`.
*   **CSS Grid e Flexbox:** Evite floats e posicionamento absoluto para estruturas de layout primárias. Use CSS Grid para grids bidimensionais complexos e Flexbox para fluxos unidimensionais.
*   **Sidebar e Layouts Principais:** Crie sidebars colapsáveis dinamicamente usando C# para gerenciar classes CSS de largura e estado do menu.

---

## 6. Acessibilidade (a11y) e SEO no Blazor

*   **Atributos ARIA:** Elementos interativos não semânticos (como `div`s com `@onclick`) devem conter `role="button"` e `tabindex="0"`.
*   **Contraste de Cores:** Garanta que a relação de contraste entre texto e fundo atenda aos padrões WCAG AA (mínimo de 4.5:1).
*   **SEO:** No Blazor, use o componente `<PageTitle>` e `<HeadContent>` para injetar títulos descritivos e meta-tags dinamicamente por página.