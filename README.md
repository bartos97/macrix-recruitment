# macrix-recruitment
## Task details
Implement an application that displays a table of people containing the following information:
- First Name,
- Last Name
- Street Name
- House Number
- Apartment Number(optional)
- PostalCode
- Town
- Phone Number
- Date ofBirth
- Age (read-only)

The expectation is to have 2 applications:
- backend serving REST API (written in the latest available to you .NET technology)
- frontend application showing UI with all the necessary information as described in specification below, by using React, Angular, WinForms, WPF, Console or any other technology that can show UI.

Complete application (FE and BE) should allowa user to edit the data, add new users and delete existing ones. The data should be persisted on disk - it can be a file or local DB, but the application must be self-contained (no external DB engines or libraries required).Below the table, there should be two buttons: "Save" and "Cancel". When the Save button is pressed changes made by a user should be persisted on disk. Pressing the "Cancel" button discards user changes and causes the table to be refreshed based on persisted already data. The buttons should be active only if the table contains unsaved data. After the first startup of the application, the table should be empty.
