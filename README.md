
# Team Name - Game Name

Unity Group 9 - Game Name

[![Game Image](https://via.placeholder.com/800x400)](https://via.placeholder.com/800x400)

# Team Members
| Name | Role | Social Media |
| --- | --- | --- |
| Bekir Evrim Sümer | Scrum Master - Developer | [Social Media] |
| Beyzanur Kahraman | Product Owner - 3D Artist | [Social Media] |
| Bahadır Işık | Developer | [Social Media] |
| Çağla Gök | Developer | [Social Media] |
| Barış Berişbek | Developer | [Social Media] |

# Game Description
### Story
Once upon a time, there was a mystical land where light and shadow danced, and colors met each other. This land was known as the "Mirror Land". However, due to an ancient curse, the beauties of this land began to fade. Light cannot find the right paths and shadows are everywhere. Brave adventurers came together to put an end to these dark forces. 

Main Story:
The game positions players as heroes who set out to illuminate this lost land. Each of them aims to progress through this dark maze using the power of light and the magic of mirrors. Players must solve complex puzzles using various mirrors and light beams and overcome obstacles that become more difficult at each level.

Characters and Roles:

Lumen: Master of Light. Lumen, the main character of the game, is an expert in directing and reflecting light. With his guidance, players develop new strategies for each level.

Mirroria: Queen of Mirrors. Throughout the game, Mirroria teaches players how to place mirrors in the best way and supports them by using her special powers.

Shadow: Master of Shadows. Shadow, the secret enemy of the game, presents new challenges at each level and tries to hinder the progress of the players.

## Game Information

- **Game Type:** Puzzle, Adventure
- **Platform:** PC
- **Game Engine:** Unity
- **Game Control:** Keyboard & Mouse
- **Art Style:** 3D

## Game Features

- **Feature 1:** Mirror Placement: Players try to direct the light to the target point by placing mirrors at certain points.

- **Feature 2:** Light Beam: To pass the level, the light beam must be reflected in the right direction.

- **Feature 3:** When the light beams reach their targets, the environment changes.

- **Feature 4:** Multiplayer: Players can solve more complex puzzles by collaborating.

- **Feature 5:** Character Control: Control the basic mechanics of the character (movement, jumping, climbing, carrying and placing mirrors).

- **Feature 6:** Cinemachine and Cinemachine Zones: Camera follows the player and changes the view according to the player's position.

## Documentation

- Task Board: [Trello Board](https://trello.com/b/bGnhI7gn/agile-board-template-trello)

# Sprints
Sprints will be 2 weeks long. At the end of each sprint, a sprint review will be held to evaluate the work done during the sprint and a refirement meeting will be held for the next sprint. Task scoring in sprints will be done with Fibonacci numbers (from 1 to 8).

Story Points:
- 1 - Very Easy
- 2 - Easy
- 3 - Medium
- 5 - Hard
- 8 - Very Hard

When estimating the time for each task, care should be taken to make these times realistic. Tasks are determined before the sprint starts and worked on these tasks during the sprint. New tasks created while the sprint is in progress should be opened in the backlog. If the developers finish their tasks, new task assignments are made from the backlog.

## Sprint 1
- **Start Date:** 24.06.2024
- **End Date:** 07.07.2024
- **Expected Story Points:** 73
- **Completed Story Points:** 68

### Sprint Goal
- Theme and story of the game will be determined.
- The software and hardware tools to be used in the development of the  game will be determined.
- User Stories will be determined and a backlog will be created.
- Generic classes and functions that the developer will use while developing the game will be created. (EventManager, UIManager, ExtensionMethods, etc.)
- Basic character movements will be implemented.
- Light Beam and Mirror Placement features will be implemented.
- Multiplayer feature will be implemented.
- Cinemachine and Cinemachine Zones will be implemented.

### Daily Scrum Meetings
  <p>We were not doing daily scrum at the beginning of the sprint and everyone was doing their own work. Therefore, our communication with each other decreased and it became difficult to follow who was doing what. Since some of our teammates had different tasks to focus on, we could not determine the voice meeting time and decided to do the daily scrum in writing via discord from June 27, 2024.</p>
  <details>
  <summary><h4 style="display: inline; margin: 0; padding: 0;">Screenshot</h4></summary>
  <img src="Screenshots/Sprint_1/daily_scrum.png" alt="Daily Scrum">
</details>

### Sprint Review
  <ul>
    <li>Worked on the theme. Multiple themes were considered and a decision was made together at the meeting.</li>
    <li>As a result of the discussions, it was decided to use Photon Unity Networking (PUN) for multiplayer and integrated into the game. Camera movements and animations were made using DOTween and Cinemachine.</li>
    <li>Basic character movements were implemented. The character can move, jump, and climb.</li>
    <li>New features that can be added to Light Beam were considered and the ideas presented were noted.</li>
    <li>The player's camera perspective was written as top-down, but it was decided that the third-person camera angle should also be tried.</li>
  </ul>

<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">Screenshot</h4></summary>
    <img src="Screenshots/Sprint_1/meeting_screenshot.png" alt="Sprint Review">
</details>

### Sprint Retrospective
#### What went well?
- The team was able to complete the sprint tasks on time.
- The team was able to decide on the theme and story of the game.
- The team was able to decide on the software and hardware tools to be used in the development of the game.

#### What didn't go well?
- The team realized that there were deficiencies in communication and collaboration in the first sprint. It was decided to communicate and collaborate more in the next sprint to eliminate these deficiencies.

#### What could be improved?
- The team decided to work more on the game's features in the next sprint.

### Sprint Backlog Creation Process and Writing Tasks

<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">Trello Board</h4></summary>
    <p>Tasks were created on the Trello board and assigned to the team members. The tasks were created by the Product Owner and the Scrum Master. The categories of the tasks created with the created tags (User Story, Feature, Bug) and the tracking of the story points were provided. </p>
    <p>The board consists of 4 main sections:</p>
    <ul>
        <li>Backlog: Lists the tasks to be done in the sprints.</li>
        <li>Sprint: Lists the tasks to be done in the sprint.</li>
        <li>Feature Review / QA: The tasks done in the sprint are tested and reviewed.</li>
        <li>Done: The tasks done in the sprint are marked as completed.</li>
    </ul>
    <img src="Screenshots/Sprint_1/trello_board.png" alt="Trello Board">
</details>

<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">Task Details</h4></summary>
    <p> After the tasks were created, the scrum master made detailed explanations and added the necessary information. In the task details, information such as which user story the task is related to, who the task is done by, and how many story points the task has were included.</p>
    <img src="Screenshots/Sprint_1/task_details.png" alt="Task Details">
</details>

### Game Screenshots
<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">Scene</h4></summary>
    <p>No scene design was done in the first sprint. However, a scene was created where the character's movements and basic mechanics were implemented.</p>
    <img src="Screenshots/Sprint_1/scene_1.png" alt="Scene">
</details>

<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">In Game</h4></summary>
    <p>Basic character movements were implemented. The character can move, jump, and climb. Also, multiplayer was tested in this scene. It was checked whether the movements were synchronized.</p>
    <img src="Screenshots/Sprint_1/game_1.png" alt="In Game 1">
    <img src="Screenshots/Sprint_1/game_2.png" alt="In Game 2">
</details>

<details>
    <summary><h4 style="display: inline; margin: 0; padding: 0;">Game Video</h4></summary>
    <p>Basic mirror placement was implemented. The player can place the mirror in the desired position and direct the light beam.</p>
    <video controls>
        <source src="Screenshots/Sprint_1/game_video_1.mp4" type="video/mp4">
        Your browser does not support the video tag.

