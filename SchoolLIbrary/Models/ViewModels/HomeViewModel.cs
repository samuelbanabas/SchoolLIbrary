namespace SchoolLIbrary.Models.ViewModels
{
    public class HomeViewModel
    {         
            public int TotalMaterials { get; set; }
            public int AvailableMaterials { get; set; }
            public int TotalStudents { get; set; }
            public int TotalLecturers { get; set; }
            public int TotalE_materials { get; set; }
            public int BorrowedMaterials { get; set; }
            public int CheckedOutMaterials { get; set; }
            public int ReturnedMaterials { get; set; }
            public List<MaterialModel>? RecentMaterials { get; set; }
            public List<MaterialModel>? TopBorrowedMaterials { get; set; }

    }
}
