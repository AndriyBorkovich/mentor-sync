### Database
please create db structure (tables, c# classes) for this module for efficient development and performance
note that I am using modular monolith architecture and each module has separated schema and separated db context, but you can still reference some data from other already implemented modules (for example from  UsersModule (add UserId but do not add navigation properties)

### Tasks
decouple tasks for this module (like user stories for jira) and extend this module functionality if you have ideas

### All functionality :
Main modules:
1.1. Users module 
This module provides work with mentor and mentee accounts. 
Registration and authorization (email/password, social networks). 
User profiles: Mentors: skills, industry, experience, programming languages, communication language. Mentees: desired skills, learning goals, communication language. User roles (mentor, mentee, administrator). Account settings management. User management (active/inactive accounts). Searching by role, name, email etc. 
1.2. Recommendations module 
Provides personalized recommendations for mentees. Algorithms based on reviews, ratings, past interactions. Using ML.NET for matrix factorization (recommendation systems). User behaviour analysis (e.g., viewed profiles). Search by industry, programming languages, experience, ratings, etc. Advanced filtering (e.g., region, availability on certain dates). Sorting results (by rating, reviews, experience).  CBF + CF.
1.3. Ratings and reviews module
Mentor-mentee rating system and feedback collection. Leaving ratings (1–5 stars). Writing text reviews. Administering reviews (filtering spam or inappropriate content). 
1.4. Scheduling and booking module
Provides interactive booking of sessions with a mentor. Calendar integration if user wants (Google Calendar, Outlook). Booking time for consultations. Manage mentor availability.  
1.5. Notifications module 
Notification system for users. Push notifications about new mentors, offers, or scheduled sessions. Email newsletters with reminders or news. Chats with mentors and mentees with ability to send photos or documents. Reminders about scheduled sessions (email, push notifications).
1.6. Learning materials module 
Provides access to additional resources. Download and save learning materials. Knowledge base with recommended books, courses, articles. User ratings of materials (implement in ratings module). Searching through all modules content (Full-text search) 

### Prompt for Figma design
**Project:** MentorSync — a web app that connects mentors and mentees in IT.  
**User Roles:** Admin, Mentor, Mentee. 
**Branding & Tone:** Professional, friendly, tech-savvy. Use a clean two-column layout on desktop, with a collapsible left nav.
 **Global Nav:**
 - Logo “MentorSync” top left
 - Collapsible sidebar with icons + labels: Dashboard, Search Mentors, My Sessions, Messages, Learning Materials, Settings
 - Top bar: notification bell, user avatar+dropdown (Profile / Logout)
 
 **1. Landing / Auth Screens**
 - **Sign Up / Login** with email/password and “Continue with Google”
 - Onboarding: Choose role (Mentor / Mentee) then fill a simple wizard (name, languages, skills/goals)
 
 **2. Mentee Dashboard**
 - **Hero**: “Recommended Mentors” carousel with card: photo, name, title, rating stars, “View Profile”
 - **Search & Filter**: industry dropdown, multiselect languages, experience slider, sort by rating/availability
 - **Activity Feed:** recent viewed mentors, upcoming sessions, new materials
 
 **3. Mentor Dashboard**
 - **Stats** cards: upcoming sessions count, average rating, pending review requests
 - **Availability Calendar**: weekly view, click to add availability slots
 - **Materials**: list of own articles with “Edit” / “Analytics”
 
 **4. Mentor Profile Page**
 - Large banner with avatar, name, “Mentor” badge, industries (tags), languages (flags), years experience
 - Tabs: About (bio, skills list), Reviews (star histogram + comments), Sessions (book now button + calendar widget), Materials (list of articles)
 
 **5. Booking Flow**
 - On mentor page: datepicker + time slots, confirm modal (with session details + cost if any)
 - After booking: confirmation screen + “Add to Google / Outlook” buttons
 
 **6. Recommendations Module UI**
 - A “My Recommendations” tab under Search: show a ranked list, each item shows CF score + “Why we recommended” tooltip (CBF match badges)

 **7. Reviews & Ratings**
 - On session completion: modal “Rate your Mentor” with 1–5 stars + optional text
 - Mentor’s Reviews tab: filter by rating, search comments
 - generate leave feedback popup on button click: header "Rate your Mentor” with 1–5 stars field + optional text comment
 
 **8. Learning Materials**
 - **Materials Library**: grid of cards (cover image, title, type badge), with a search bar
 - **Material Detail**: markdown article content, download attachments list, star rating widget
 
 **9. Chats**
 - 
 
 **10. Admin Panel**
 - Under a special route: Analytics dashboard (user counts, sessions per day, average ratings), User management table (searchable, filter by role/status, enable/disable buttons)
 
 **Interactions & States:**
 - Loading skeletons for data fetch
 - Inline validations on forms (e.g. “Experience must be ≥ 0”)
 - Success / error toasts on save actions    
 
 **Responsive Notes:**
 - Collapse sidebar to icon-only on tablet, move nav into a hamburger menu on mobile
 - Search filters collapse into a slide-in panel on small screens
 
 **Deliver:**
 - High-fidelity desktop mockups for each core screen above
 - A consistent component library: buttons, form fields, cards, avatars, tags, calendars, rating stars