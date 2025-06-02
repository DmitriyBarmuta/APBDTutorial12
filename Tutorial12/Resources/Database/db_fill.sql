INSERT INTO dbo.Country (Name)
VALUES ('Poland'),
       ('Germany'),
       ('Italy');


INSERT INTO dbo.Trip (Name, Description, DateFrom, DateTo, MaxPeople)
VALUES ('Poland Spring 2025',
        'Explore Kraków & Warsaw in spring.',
        '2025-04-01T00:00:00',
        '2025-04-07T00:00:00',
        20),
       ('Rhine River Cruise',
        'Seven-day cruise from Basel to Amsterdam',
        '2025-06-10T00:00:00',
        '2025-06-17T00:00:00',
        15),
       ('Italian Food Tour',
        'Taste your way through Tuscany & Milan',
        '2025-07-05T00:00:00',
        '2025-07-12T00:00:00',
        12);


INSERT INTO dbo.Client (FirstName, LastName, Email, Telephone, Pesel)
VALUES ('Alice', 'Novák', 'alice.novak@example.com', '+48-123-456-789', '86010112345'),
       ('Bob', 'Müller', 'bob.mueller@example.com', '+49-987-654-321', '90020254321'),
       ('Carla', 'Rossi', 'carla.rossi@example.com', '+39-333-444-555', '92030398765');


INSERT INTO dbo.Country_Trip (IdCountry, IdTrip)
VALUES (1, 1),
       (2, 2),
       (3, 3);


INSERT INTO dbo.Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES (1, 1, '2025-02-01T10:30:00', NULL),
       (2, 2, '2025-05-01T14:45:00', '2025-05-05T09:00:00'),
       (3, 3, '2025-05-15T11:20:00', NULL);