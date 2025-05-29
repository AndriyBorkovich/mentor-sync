export interface Material {
    id: string;
    title: string;
    description: string;
    type: "document" | "video" | "link" | "presentation";
    sessionId?: string;
    mentorName: string;
    createdAt: string;
    tags: string[];
    thumbnail?: string;
    content?: string;
    url?: string;
    fileSize?: string;
}

export const materials: Material[] = [
    {
        id: "1",
        title: "Основи React: компоненти та стан",
        description:
            "Детальний огляд основних концепцій React - створення компонентів, управління станом та життєвий цикл.",
        type: "document",
        mentorName: "Оксана Лень",
        createdAt: "15 травня 2025",
        tags: ["React", "Frontend", "JavaScript"],
        fileSize: "2.4 MB",
        content: `# Основи React: компоненти та стан

## Що таке React?

React - це JavaScript бібліотека для створення користувацьких інтерфейсів. 
Розроблена Facebook, вона дозволяє створювати складні інтерфейси з ізольованих частин коду, які називаються "компонентами".

## Основні концепції React

### Компоненти

Компоненти - це незалежні, повторно використовувані частини коду. Вони працюють як JavaScript функції, 
приймаючи довільні входи (так звані "props") і повертають React елементи, які описують що має відображатись на екрані.

\`\`\`jsx
function Welcome(props) {
  return <h1>Привіт, {props.name}</h1>;
}
\`\`\`

### JSX

JSX - це синтаксичне розширення JavaScript. Воно схоже на HTML, але має всю потужність JavaScript.

\`\`\`jsx
const element = <h1>Привіт, світ!</h1>;
\`\`\`

### Стан і життєвий цикл

React компоненти можуть мати "стан", який визначає як компонент виглядає і поводиться.

\`\`\`jsx
class Clock extends React.Component {
  constructor(props) {
    super(props);
    this.state = {date: new Date()};
  }

  componentDidMount() {
    this.timerID = setInterval(
      () => this.tick(),
      1000
    );
  }

  componentWillUnmount() {
    clearInterval(this.timerID);
  }

  tick() {
    this.setState({
      date: new Date()
    });
  }

  render() {
    return (
      <div>
        <h1>Привіт, світ!</h1>
        <h2>Зараз {this.state.date.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit', hour12: false})}.</h2>
      </div>
    );
  }
}
\`\`\`

### React Hooks

Hooks дозволяють використовувати стан та інші можливості React без написання класів.

\`\`\`jsx
import React, { useState, useEffect } from 'react';

function Example() {
  // Оголошення змінної стану "count"
  const [count, setCount] = useState(0);

  // Подібно до componentDidMount та componentDidUpdate
  useEffect(() => {
    document.title = \`Ви клікнули \${count} разів\`;
  });

  return (
    <div>
      <p>Ви клікнули {count} разів</p>
      <button onClick={() => setCount(count + 1)}>
        Клікніть мене
      </button>
    </div>
  );
}
\`\`\`

## Висновок

React змінив спосіб, яким ми розробляємо веб-інтерфейси. 
Використовуючи принципи компонентної архітектури та ефективного управління станом, 
React дозволяє створювати складні інтерфейси, які легко підтримувати та розширювати.`,
    },
    {
        id: "2",
        title: "Введення в TypeScript",
        description:
            "Базові концепції TypeScript - типи даних, інтерфейси, класи та інше.",
        type: "presentation",
        sessionId: "101",
        mentorName: "Михайло Янків",
        createdAt: "10 травня 2025",
        tags: ["TypeScript", "JavaScript", "Frontend"],
        thumbnail: "/presentation-thumbnail.jpg",
        fileSize: "4.7 MB",
    },
    {
        id: "3",
        title: "Побудова RESTful API з Node.js та Express",
        description:
            "Покроковий посібник з розробки RESTful API за допомогою Node.js і Express.",
        type: "video",
        mentorName: "Петро Петрів",
        createdAt: "5 травня 2025",
        tags: ["Node.js", "Express", "API", "Backend"],
        url: "https://www.youtube.com/watch?v=example",
        thumbnail: "/video-thumbnail.jpg",
    },
    {
        id: "4",
        title: "Документація по MongoDB",
        description: "Офіційна документація MongoDB з прикладами використання",
        type: "link",
        mentorName: "Дмитро Кім",
        createdAt: "1 травня 2025",
        tags: ["MongoDB", "Database", "NoSQL"],
        url: "https://docs.mongodb.com",
    },
    {
        id: "5",
        title: "Архітектура мікросервісів",
        description:
            "Принципи проектування та імплементації мікросервісної архітектури",
        type: "document",
        mentorName: "Оксана Лень",
        createdAt: "28 квітня 2025",
        tags: ["Architecture", "Microservices", "System Design"],
        fileSize: "3.1 MB",
    },
    {
        id: "6",
        title: "Основи Docker: від А до Я",
        description:
            "Повний посібник з використання Docker для контейнеризації додатків",
        type: "video",
        mentorName: "Михайло Янків",
        createdAt: "25 квітня 2025",
        tags: ["Docker", "DevOps", "Containers"],
        url: "https://www.youtube.com/watch?v=example2",
        thumbnail: "/video-thumbnail2.jpg",
    },
];
