# HomeNest – Кратка документация

## 1. Обобщение

**HomeNest** е уеб платформа за обяви за недвижими имоти (апартаменти, къщи, пентхауси). Проектът дава възможност за разглеждане на оферти, детайлна информация за всеки имот, публикуване на собствени обяви, управление на профил, списък с любими имоти и абонамент за бюлетин.

---

## 2. Технологичен стек

### 2.1 Основно приложение – `HomeNest/` (активно)

| Технология | Версия / Описание |
|------------|-------------------|
| **.NET SDK** | 9.0 |
| **ASP.NET Core** | Razor Components (Blazor) с **Interactive Server** рендиране |
| **Език** | C# 12 (Nullable enabled, Implicit usings) |
| **Styling** | CSS изолиран на ниво компонент (`.razor.css`) + глобални стилове в `wwwroot` |
| **Статични активи** | Изображения, SVG икони, шрифтове |
| **HTTP / Security** | Вградени ASP.NET Core middleware – HTTPS редирект, Antiforgery, HSTS |


## 3. Архитектура и структура

### 3.1 `HomeNest/` – Blazor Server приложение

```
HomeNest/
├── Program.cs                      # Точка на вход – DI, middleware pipeline
├── appsettings.json                # Конфигурация
├── appsettings.Development.json    # Dev конфигурация
├── HomeNest.csproj                 # .NET проектен файл
├── Components/
│   ├── App.razor                   # Root компонент
│   ├── Routes.razor                # Routing
│   ├── _Imports.razor              # Глобални using директиви
│   ├── AuthModal.razor             # Модал за автентикация
│   ├── NewsletterSection.razor     # Секция за абонамент
│   ├── Icons/                      # SVG икони (Razor компоненти)
│   │   ├── ArrowLeft.razor
│   │   ├── ArrowRight.razor
│   │   ├── Edit.razor
│   │   ├── Messages.razor
│   │   ├── SmsTracking.razor
│   │   └── Star.razor
│   ├── Layout/
│   │   ├── MainLayout.razor        # Главен layout
│   │   ├── MainLayout.razor.css    # Изолирани стилове
│   │   ├── NavMenu.razor           # Навигационно меню
│   │   ├── NavMenu.razor.css
│   │   └── Footer.razor            # Футър
│   └── Pages/                      # Страници (маршрути)
│       ├── Home.razor              # "/" – Landing page
│       ├── About.razor             # "/about"
│       ├── Contact.razor           # "/contact"
│       ├── Login.razor             # "/login"
│       ├── Favorites.razor         # "/favorites"
│       ├── MyListings.razor        # "/mylistings"
│       ├── Profile.razor           # "/profile"
│       ├── PublishProperty.razor   # "/publish"
│       ├── EditProperty.razor      # "/edit/{id}"
│       ├── PropertyDetailPage.razor # "/property/{id}"
│       └── Error.razor             # "/Error"
├── Services/
│   ├── PropertyService.cs          # Бизнес логика за имоти (in-memory)
│   └── UserStateService.cs         # Управление на потребителско състояние
└── wwwroot/                        # Статични файлове (CSS, JS, изображения)
```


## 4. Основни функционалности

| Модул | Описание |
|-------|----------|
| **Начална страница** | Hero секция, предимства, скорошни оферти, отзиви, абонамент за бюлетин |
| **Търсене / Каталог** | Разглеждане на налични обяви |
| **Детайли за имот** | Пълна информация за избран имот (`/property/{id}`) |
| **Публикуване / Редакция** | Форми за създаване и редактиране на обяви |
| **Потребителски профил** | Личен кабинет, любими имоти, мои обяви |
| **Автентикация** | Логин / регистрация чрез `AuthModal` |
| **Статично съдържание** | Страници „За нас“ и „Контакти“ |

---

## 5. Услуги (Services)

| Сервиз | Роля |
|--------|------|
| `PropertyService` | In-memory управление на обяви за имоти – CRUD операции |
| `UserStateService` | Управление на потребителска сесия и състояние |

*Забележка: В момента няма интеграция с база данни – данните се пазат в паметта (Singleton).*

--

## 7. Бележки

- **Основният продукт** е Blazor Server приложението в папка `HomeNest/`.
- Изображенията се държат в `wwwroot/images/` за Blazor и в `src/assets/images/` за React.
- UI компонентите в `src/components/ui/` следват конвенцията на **shadcn/ui** (Radix UI + Tailwind).
