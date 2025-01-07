SELECT 
    s.StudentID,
    s.StudentNumber,
    s.Name,
    s.Title,
    sd.Goal,
    sd.Level,
    sd.FirstLesson,
    sd.LessonsTotal,
    sd.LastLesson,
    sd.StudyStatus,
    cd.CityOfResidence,
    cd.CountryOfResidence,
    cd.ProvinceOfResidence,
    cd.AddressField,
    cd.Email1,
    cd.Email2,
    cd.Tel1,
    cd.Tel2,
    cd.TelOrtu1,
    cd.TelOrtu2,
    pd.Age,
    pd.Gender,
    pd.CountryOfOrigin,
    pd.ProvinceOfOrigin,
    pd.CityOfOrigin
FROM 
    students s
LEFT JOIN 
    studydetails sd ON s.StudentID = sd.StudentID
LEFT JOIN 
    contactdetails cd ON s.StudentID = cd.StudentID
LEFT JOIN 
    personaldetails pd ON s.StudentID = pd.StudentID;