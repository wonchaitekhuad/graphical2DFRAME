# คู่มือการใช้งานฟีเจอร์รับพิกัด X, Y

## ภาพรวม
ฟีเจอร์นี้เพิ่มความสามารถในการรับค่าพิกัด X และ Y จากผู้ใช้ในฟอร์มหลัก (Form1) ของโปรแกรม Graphical 2D Frame Analysis

## คุณสมบัติหลัก

### 1. UI Components
- **TextBox txtX**: สำหรับป้อนค่าพิกัด X
- **TextBox txtY**: สำหรับป้อนค่าพิกัด Y
- **Button btnSubmit**: ปุ่ม "ยืนยันพิกัด" สำหรับส่งค่า
- **Label lblResult**: แสดงผลลัพธ์หลังจากยืนยันค่า

### 2. การ Parse ข้อมูลอย่างปลอดภัย
- รองรับทั้งจุดทศนิยมแบบ `.` (dot) และ `,` (comma)
- ใช้ `double.TryParse` กับทั้ง `CultureInfo.InvariantCulture` และ `CultureInfo.CurrentCulture`
- ป้องกัน exception ที่อาจเกิดขึ้นด้วย try-catch block

### 3. การแจ้งเตือนและ Validation
- แสดง MessageBox เมื่อข้อมูลไม่ถูกต้อง
- แสดงข้อความเป็นภาษาไทยที่เข้าใจง่าย
- แสดงผลลัพธ์สีเขียวเมื่อรับค่าสำเร็จ

### 4. Event System
- มี Event `PointReceived(double x, double y)` ที่ถูก raise เมื่อรับค่าสำเร็จ
- ส่วนอื่นของโปรแกรมสามารถ subscribe event นี้เพื่อรับค่าพิกัดไปใช้งานต่อได้

## วิธีการใช้งาน

### สำหรับผู้ใช้
1. เปิดโปรแกรม Graphical 2D Frame Analysis
2. มองหาช่องป้อนข้อมูล "X:" และ "Y:" ที่มุมบนขวาของฟอร์ม
3. ป้อนค่าพิกัด X และ Y (รองรับทั้ง `10.5` และ `10,5`)
4. คลิกปุ่ม "ยืนยันพิกัด"
5. ดูผลลัพธ์ที่แสดงใต้ปุ่ม

### สำหรับนักพัฒนา

#### การ Subscribe Event PointReceived
```csharp
// ใน Form1 หรือ class อื่นที่ต้องการรับค่าพิกัด
form1Instance.PointReceived += (x, y) => {
    // ใช้ค่า x และ y ที่ได้
    Console.WriteLine($"Received coordinates: X={x}, Y={y}");
    // ... ทำงานอื่นๆ ตามต้องการ
};
```

#### ตัวอย่างการใช้งานใน Class อื่น
```csharp
public class CoordinateHandler
{
    public void AttachToForm(Form1 form)
    {
        form.PointReceived += OnPointReceived;
    }
    
    private void OnPointReceived(double x, double y)
    {
        // ประมวลผลพิกัดที่ได้รับ
        ProcessCoordinates(x, y);
    }
    
    private void ProcessCoordinates(double x, double y)
    {
        // logic สำหรับประมวลผลพิกัด
    }
}
```

## การทดสอบ

### Test Cases
1. **ค่าปกติ (Dot decimal)**: ป้อน `10.5` และ `20.5` → ควรแสดง "พิกัดที่รับ: X = 10.5, Y = 20.5"
2. **ค่าปกติ (Comma decimal)**: ป้อน `10,5` และ `20,5` → ควรแสดงผลเหมือนข้างบน
3. **ค่าไม่ถูกต้อง**: ป้อน `abc` หรือ `!@#` → ควรแสดง MessageBox แจ้งเตือน
4. **ช่องว่าง**: ไม่ป้อนค่าใด → ควรแสดง MessageBox แจ้งเตือน

## ข้อกำหนดทางเทคนิค
- .NET Framework 4.7.2
- Windows Forms
- ภาษา C#
- ใช้ System.Globalization สำหรับ CultureInfo

## ความปลอดภัย
- ใช้ `double.TryParse` เพื่อป้องกัน parsing exception
- มี try-catch block เพื่อจัดการ exception ที่ไม่คาดคิด
- แสดง error message ที่เป็นมิตรต่อผู้ใช้
- ไม่มีการ swallow exception โดยไม่แจ้งเตือน

## การบำรุงรักษา
- Code อยู่ในไฟล์ `Form1.cs` (method `BtnSubmit_Click`)
- UI controls ถูกประกาศในไฟล์ `Form1.Designer.cs`
- ใช้ naming convention ตามมาตรฐาน: txtX, txtY, btnSubmit, lblResult

## Known Issues
ไม่มี

## Future Enhancements
- เพิ่มการแสดงพิกัดบน graphical canvas
- เพิ่มการเก็บ history ของพิกัดที่เคยป้อน
- เพิ่ม validation เพิ่มเติม (เช่น range checking)
