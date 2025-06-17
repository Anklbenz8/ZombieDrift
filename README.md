# ZombieDrift
на прототип на тест мертик

1. Две сцены Garage и Gameplay 

В проекте используется DI контейнер (Zenject)

Сценой управляет стейт машина

Entry - запускает GameplayScenario
Состояния Gameplay лежат тут 
\Assets\ZombieDrift\Scripts\GameplayScene\GameScenario\States

ConstructState - инициализация, загрузка сохраниений. Создание ботов на текущей карте, нужной карты, нужной машины
MenuState - состояние главного меню (Шлавное меню на фоне карты и выбраного авто)
GetReadyState - онбординг в игру Ui онбординга
GameplayState - запуск игры, определение выиграл/проиграл, подсчет очков и прочее
PauseState - сотояние паузы (пауза работает чере pauseService ( содержит все обьекты который IPauseSensitive не обновляет их есть isPause = true)
WinState - Ui выигрыша, сохраниение
LoseState - Ui проигрыша, сохраниение 
RepairState - В случае есть пользователь покупает возражение, удаляет разбитое авто, создает новой -> после переходит в GetreadyState 
FinalizeState  - удаление карты, ботов авто и прочего

Сохранение SaveLoadSystem, AdsSystem  - реализованны через Стратегию
Создание эффетов, хитнов, партиктов и пр. -> pool of obj
Для настройки всего - используются конфиги в виде SO все тут -> ZombieDrift/Config

Видео gameplay https://youtu.be/FUUzK1Hbu0I?si=vcMRpo7qw_8_YODB