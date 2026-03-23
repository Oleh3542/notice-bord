USE AnnouncementDB; 
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spGetAnnouncements')
    DROP PROCEDURE spGetAnnouncements;
GO

CREATE PROCEDURE spGetAnnouncements
AS
BEGIN
    SELECT 
        Id, 
        Title, 
        Description, 
        CreatedDate, 
        Status, 
        Category, 
        SubCategory
    FROM Announcements
    ORDER BY CreatedDate DESC;
END
GO