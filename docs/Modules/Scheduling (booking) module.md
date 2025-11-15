# Scheduling Module

Provides interactive session booking with mentors.

-   Calendar integration (Google Calendar, Outlook).
-   Time booking for consultations.
-   Management of mentor availability.

    **MATCH-001 Create a Mentor-Mentee Relations Table**
    \MentorMenteeRelation\ table:

-   \Id (PK)\, \MentorId (FK)\, \MenteeId (FK)\, \Status (Pending/Accepted/Rejected)\.

**MATCH-002 Implement Mentor-Mentee Request API**
 \POST /api/mentorship/request\ (mentee requests a mentor).
 \POST /api/mentorship/accept\ (mentor approves request).
 \POST /api/mentorship/reject\ (mentor rejects request).

**MATCH-003 Implement Mentor Availability API**
 Create a \MentorAvailability\ table with available time slots.
 Implement API \GET /api/mentors/availability\.
