## Описание сцен
Проект включает две основные сцены:
- `Garage`
- `Gameplay`

Для внедрения зависимостей используется **Zenject**.

## Управление сценой через стейт-машину
Сцена `Gameplay` управляется стейт-машиной. Состояния находятся в:  
`/Assets/ZombieDrift/Scripts/GameplayScene/GameScenario/States`

### Состояния GameplayScenario
| Состояние | Описание |
|-----------|----------|
| **`Entry`** | Запускает `GameplayScenario` |
| **`ConstructState`** | Инициализация, загрузка сохранений, создание ботов, загрузка карты и машины |
| **`MenuState`** | Главное меню (на фоне карты с выбранным авто) |
| **`GetReadyState`** | Онбординг перед игрой (UI) |
| **`GameplayState`** | Основной геймплей: механики, подсчёт очков, определение победы/поражения |
| **`PauseState`** | Пауза (через `PauseService`, который управляет объектами с `IPauseSensitive`) |
| **`WinState`** | UI победы + сохранение прогресса |
| **`LoseState`** | UI поражения + сохранение прогресса |
| **`RepairState`** | Восстановление машины (удаление старой → создание новой → переход в `GetReadyState`) |
| **`FinalizeState`** | Очистка: удаление карты, ботов, машин |

- **`SaveLoadSystem`** и **`AdsSystem`** → реализованы через паттерн **Стратегия**
- Эффекты/партиклы → **Pool of Objects**
- Настройки → **Scriptable Objects** (папка `ZombieDrift/Config`)

## Геймплей
Видео: [YouTube](https://youtu.be/FUUzK1Hbu0I?si=vcMRpo7qw_8_YODB)
