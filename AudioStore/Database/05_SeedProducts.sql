-- =====================================================================
-- 05_SeedProducts.sql
-- Nhap hang loat 38 san pham (#23 -> #60) vao DB_AudioStore.
-- 22 san pham dau (#1 -> #22: tai nghe In-ear / Over-ear, Marshall Kilburn II, Middleton) da duoc nhap tay qua trang admin -> KHONG nam trong script nay.
-- Chay SAU 01..04. Co the chay lai nhieu lan (bo qua san pham da ton tai theo ProductName).
--
-- CategoryID : 1 Over-ear | 2 In-ear | 3 Loa to | 4 Loa xach tay | 5 Soundbar | 6 Mic Studio | 7 Mic Karaoke
-- StockQuantity: dat mac dinh 50 cho moi san pham (sua o cot StockQuantity neu can).
-- ImageUrl : 3 link anh, ngan cach bang dau '|'  (doi bien @Sep o PHAN 2 neu web cua ban dung dau khac)
-- VideoUrl : 1 link video
-- LUU Y: Luu file voi encoding UTF-8 (BOM) de khong loi font tieng Viet.
-- =====================================================================
USE DB_AudioStore;
GO

SET NOCOUNT ON;

-- PHAN 1: THONG TIN SAN PHAM
INSERT INTO Products (ProductName, Price, StockQuantity, CategoryID, BrandID, IsWireless, Specifications, Description, IsActive)
SELECT v.ProductName, v.Price, v.StockQuantity, v.CategoryID, b.BrandID, v.IsWireless, v.Specifications, v.Description, 1
FROM (VALUES

    -- ---- LOA XACH TAY ----
    -- 23
    (N'Marshall Emberton II', 3990000, 50, 4, N'Marshall', 1,
     N'Bluetooth 5.1, Công suất 20W Class D, Pin 30+ giờ, Chuẩn chống nước/bụi IP67, Công nghệ True Stereophonic 360 độ, Hỗ trợ Stack Mode, Trọng lượng nhẹ 700g.',
     N'Marshall Emberton II là phiên bản nâng cấp hoàn hảo của mẫu loa di động bán chạy nhất hãng. Kích thước chỉ vừa bằng một gạch nhỏ nhưng vỏ ngoài được bọc cao su vân da bền bỉ, giúp chống chọi tốt với va đập và thời tiết khắc nghiệt.

Sử dụng công nghệ âm thanh đa hướng True Stereophonic độc quyền của Marshall, Emberton II mang đến trải nghiệm âm thanh 360 độ hoàn toàn vượt trội so với kích thước nhỏ bé của nó. Thời lượng pin được kéo dài lên đến hơn 30 giờ chỉ với một lần sạc, giúp cuộc vui âm nhạc của bạn không bao giờ ngắt gián.'),
    -- 24
    (N'Marshall Willen', 2490000, 50, 4, N'Marshall', 1,
     N'Bluetooth 5.1, Driver toàn dải 2 inch 10W + 2 Màng cộng hưởng thụ động, Pin 15+ giờ, Chuẩn chống nước/bụi IP67, Quai gài cao su đa năng phía sau, Tích hợp Micro đàm thoại.',
     N'Marshall Willen là chiếc loa di động nhỏ nhất gia đình Marshall, thiết kế dạng vuông nằm gọn trong lòng bàn tay. Mặt sau loa được tích hợp một sợi quai cao su co giãn thông minh, cho phép bạn dễ dàng móc loa vào ghi-đông xe đạp, quai balo hay treo lên cành cây trong các chuyến dã ngoại.

Dù kích thước siêu nhỏ, Willen vẫn tái hiện âm thanh rõ ràng, mượt mà với dải trung ấm áp đặc trưng. Loa tích hợp sẵn micro đàm thoại chất lượng cao, giúp bạn nhận các cuộc gọi rảnh tay nhanh chóng mà không cần ngắt kết nối với điện thoại.'),
    -- 25
    (N'JBL Charge 5', 3490000, 50, 4, N'JBL', 1,
     N'Bluetooth 5.1, Công suất 40W RMS (Woofer 30W + Tweeter 10W), Pin 20 giờ (7500mAh), Chuẩn chống nước/bụi IP67, Tích hợp sạc dự phòng Powerbank qua cổng USB-A, Công nghệ JBL PartyBoost.',
     N'JBL Charge 5 sở hữu thiết kế dạng trụ tròn khỏe khoắn, được bao bọc bởi lớp vải dệt chống mòn và các gờ cao su bảo vệ ở hai đầu màng loa thụ động. Sản phẩm sẵn sàng đồng hành cùng bạn ra bãi biển, hồ bơi hay những chuyến leo núi mạo hiểm mà không lo bám bụi bẩn hay ngấm nước.

Cấu hình loa cải tiến gồm một củ loa woofer dạng racetrack, loa tweeter độc lập và hai màng cộng hưởng âm trầm hai đầu giúp Charge 5 phát ra âm thanh mạnh mẽ, dải bass uy lực. Pin dung lượng khủng 7500mAh đóng vai trò như một viên sạc dự phòng cung cấp năng lượng cho điện thoại của bạn ngay trong lúc phát nhạc.'),
    -- 26
    (N'JBL Flip 6', 2490000, 50, 4, N'JBL', 1,
     N'Bluetooth 5.1, Hệ thống loa 2 đường tiếng (Woofer 20W + Tweeter 10W), Pin 12 giờ, Chuẩn chống nước/bụi IP67, Bảo vệ sạc USB-C thông minh, Hỗ trợ ứng dụng JBL Portable.',
     N'JBL Flip 6 là mẫu loa di động cầm tay nhỏ gọn cực kỳ thành công của hãng. Thiết kế dạng ống cầm vừa tay, trang bị dây xỏ tiện lợi để treo trên tường hoặc mang theo bên người.

Khác biệt lớn so với thế hệ trước là hệ thống loa 2 đường tiếng mới gồm một củ loa tweeter riêng biệt kết hợp củ loa racetrack woofer, đem lại chi tiết âm cao trong trẻo hơn và âm trầm sâu lắng hơn. Chuẩn chống bụi/nước IP67 giúp bạn tự tin sử dụng loa ở mọi địa hình môi trường.'),
    -- 27
    (N'JBL Xtreme 4', 7290000, 50, 4, N'JBL', 1,
     N'Bluetooth 5.3 với Auracast, Công suất 100W RMS, Pin 24 giờ (khả năng tháo rời thay thế), Thuật toán tối ưu AI Sound Boost, Dây đeo vai đi kèm khui mở nắp chai, Chuẩn chống nước/bụi IP67.',
     N'JBL Xtreme 4 được mệnh danh là "quái vật" âm thanh xách tay dành cho các buổi dã ngoại ngoài trời. Loa trang bị quai đeo vai chắc chắn tích hợp sẵn móc mở nắp chai tiện lợi, giúp bạn dễ dàng khoác loa lên vai và khuấy đảo mọi không gian âm nhạc.

Công nghệ AI Sound Boost sử dụng trí tuệ nhân tạo phân tích tín hiệu âm thanh theo thời gian thực để tối ưu hóa công suất đầu ra, giảm thiểu méo tiếng và đẩy âm bass lên tầm cao mới. Khả năng tháo rời viên pin giúp người dùng dễ dàng thay thế pin dự phòng cho những chuyến đi dài ngày.'),
    -- 28
    (N'Sony ULT FIELD 1', 2990000, 50, 4, N'Sony', 1,
     N'Bluetooth 5.3, Nút bấm ULT POWER SOUND tăng bass, Pin 12 giờ, Chuẩn kháng bụi/nước IP67, Kháng nước biển & Chống va đập, Quai dây xách đa năng, Đặt đứng hoặc nằm ngang.',
     N'Sony ULT FIELD 1 là loa di động nhỏ gọn thế hệ mới được thiết kế đặc biệt cho lối sống năng động. Thiết kế có thể đặt đứng hoặc nằm ngang cùng quai xách dây dù đa năng giúp bạn linh hoạt treo loa vào balo, lều cắm trại hay khung xe đạp.

Chỉ cần nhấn nút ULT, chiếc loa ngay lập tức kích hoạt chế độ tăng cường âm trầm, tạo ra những nhịp đập bass lực và sâu bất ngờ. Loa đạt chuẩn IP67 với khả năng chống nước biển và chống sốc va đập, đảm bảo hoạt động bền bỉ trong mọi môi trường dã ngoại khắc nghiệt.'),
    -- 29
    (N'Sony SRS-XB100', 1190000, 50, 4, N'Sony', 1,
     N'Bluetooth 5.3, Màng loa lệch tâm (Off-center Diaphragm), Bộ xử lý khuếch đại âm thanh Sound Diffusion Processor, Pin 16 giờ, Chuẩn IP67, Vỏ loa sử dụng nhựa tái chế, Dây đeo hai chiều.',
     N'Sony SRS-XB100 là chiếc loa di động mini cực kỳ xinh xắn dành cho cá nhân. Kích thước vô cùng nhỏ gọn nằm trọn trong lòng bàn tay cùng dây đeo hai chiều tiện lợi giúp bạn mang âm nhạc theo bên mình đến bất cứ đâu.

Dù nhỏ bé nhưng SRS-XB100 sở hữu bộ xử lý khuếch đại âm thanh Sound Diffusion Processor giúp lan tỏa âm thanh rộng khắp mọi hướng. Thiết kế màng loa lệch tâm cải thiện độ trong trẻo của dải trung và giọng hát, đem lại trải nghiệm âm thanh chất lượng cao vượt xa mong đợi.'),
    -- 30
    (N'Harman Kardon Onyx Studio 8', 5990000, 50, 4, N'Harman Kardon', 1,
     N'Bluetooth 5.2, Công suất 50W RMS, Loa Woofer 120mm + 2 Tweeter 20mm, Tay cầm nhôm Anodized đóng vai trò chân đế, Pin 8 giờ, Tự động căn chỉnh âm thanh theo phòng (Self-Tuning), Micro kép đàm thoại.',
     N'Harman Kardon Onyx Studio 8 mang thiết kế dáng hành tinh tròn độc đáo với vòng tay cầm bằng nhôm đúc nguyên khối bao quanh, vừa làm quai xách sang trọng vừa làm chân đế vững chắc. Toàn bộ vỏ loa được bọc vải lưới dệt cao cấp làm từ 100% sợi polyester tái chế.

Mỗi khi bạn bật nguồn và di chuyển loa sang một vị trí mới, tính năng Self-Tuning thông minh sẽ tự động đo đạc cấu trúc phòng để tối ưu hóa âm thanh đầu ra. Chất âm Onyx Studio 8 nổi tiếng với dải bass sâu thẳm, âm trung ngọt ngào và âm cao thanh thoát, làm nổi bật mọi không gian sống hiện đại.'),

    -- ---- LOA TO ----
    -- 31
    (N'Marshall Woburn III', 12990000, 50, 3, N'Marshall', 1,
     N'Bluetooth 5.2 LE, Công suất 150W RMS Class D (Woofer 90W + 2 Mid 15W + 2 Tweeter 15W), Cổng cắm HDMI ARC, RCA, 3.5mm, Cắm điện trực tiếp 220V, Công nghệ Dynamic Loudness.',
     N'Marshall Woburn III là mẫu loa cắm điện gia đình cỡ lớn nhất của hãng. Sở hữu phong cách Vintage lịch lãm với lớp vỏ bọc da, mặt lưới dệt cổ điển và các núm chỉnh bằng đồng thau mạ vàng kim sang trọng, Woburn III là điểm nhấn nội thất nâng tầm đẳng cấp cho phòng khách nhà bạn.

Hệ thống loa 3 đường tiếng cải tiến mang đến âm trường rộng mở hơn bao giờ hết, dải trầm xuống cực sâu làm rung chuyển căn phòng. Đặc biệt, cổng kết nối HDMI ARC cho phép bạn dễ dàng kết nối loa trực tiếp với TV để nâng cấp trải nghiệm xem phim và giải trí gia đình chất lượng cao.'),
    -- 32
    (N'Marshall Stanmore III', 8990000, 50, 3, N'Marshall', 1,
     N'Bluetooth 5.2, Công suất 80W RMS (Woofer 50W + 2 Tweeter 15W), Tweeter hướng góc ngoài, Cổng cắm RCA và 3.5mm, Cắm điện trực tiếp, Công nghệ Placement Compensation tối ưu vị trí đặt loa.',
     N'Marshall Stanmore III là mẫu loa cắm điện tầm trung bán chạy nhất của Marshall nhờ kích thước vừa vặn cho hầu hết các không gian phòng khách và phòng làm việc. Thiết kế giữ trọn vẻ đẹp hoài cổ đặc trưng của những chiếc ampli đĩa than cổ điển.

Màn loa Tweeter được đặt góc hướng ra ngoài kết hợp ống dẫn âm cải tiến giúp âm thanh lan tỏa rộng khắp không gian. Tính năng Placement Compensation trong ứng dụng Marshall Bluetooth giúp điều chỉnh âm thanh dựa trên vị trí bạn đặt loa (gần tường hay góc phòng), đảm bảo chất âm luôn tròn trịa.'),
    -- 33
    (N'JBL PartyBox 710', 17990000, 50, 3, N'JBL', 0,
     N'Bluetooth 5.1, Công suất siêu khủng 800W RMS, 2 Loa Woofer 8 inch + 2 Tweeter 2.75 inch, Hệ thống đèn LED hình số 8 đồng bộ nhịp điệu, Kháng nước IPX4, Cổng cắm Nhạc cụ & Micro, Bánh xe và tay cầm di chuyển.',
     N'JBL PartyBox 710 là cỗ máy âm thanh tiệc tùng đỉnh cao dành cho các không gian lớn, sân vườn hay biệt thự. Sản phẩm được trang bị bánh xe chịu lực chắc chắn và tay cầm cứng cáp giúp bạn dễ dàng di chuyển khối loa công suất khủng này đến bất kỳ vị trí nào.

Công suất 800W RMS kết hợp công nghệ JBL Original Pro Sound đem đến âm trầm dội bùng nổ mà không bị méo tiếng. Hệ thống trình diễn ánh sáng LED số 8 với các hiệu ứng chớp nháy, ánh sao và hiệu ứng nhịp điệu đồng bộ hoàn hảo theo điệu nhạc, tạo nên bầu không khí lễ hội sôi động.'),
    -- 34
    (N'JBL PartyBox 310', 12990000, 50, 3, N'JBL', 1,
     N'Bluetooth 5.1, Công suất 240W RMS, Pin dung lượng 18 giờ phát nhạc, Đèn LED tiệc tùng đa màu sắc, Kháng nước IPX4, Thiết kế dạng vali kéo có bánh xe, Cổng kết nối Guitar & Micro có nút chỉnh Echo.',
     N'JBL PartyBox 310 kết hợp hoàn hảo giữa công suất âm thanh uy lực và tính di động linh hoạt nhờ tay kéo thu gọn dạng vali cùng bánh xe mượt mà. Mặt trên loa tích hợp bảng điều khiển có đèn nền giúp bạn thao tác chỉnh nhạc dễ dàng ngay cả trong đêm tối.

Với thời lượng pin lên tới 18 giờ, bạn có thể thỏa thích ca hát và nhảy múa suốt từ chiều đến đêm muộn mà không phụ thuộc vào nguồn điện lưới. Cổng cắm Micro và Guitar tích hợp các hiệu ứng chỉnh giọng Echo/Treble/Bass biến chiếc loa thành một dàn karaoke di động chuyên nghiệp.'),
    -- 35
    (N'JBL PartyBox Club 120', 9990000, 50, 3, N'JBL', 1,
     N'Bluetooth 5.3 với Auracast, Công suất 160W RMS, Pin 12 giờ (tháo rời thay thế được), Thuật toán tối ưu AI Sound Boost, Trình diễn ánh sáng LED tương tác, Kháng nước IPX4, Tay cầm gập gọn bọc silicon.',
     N'JBL PartyBox Club 120 nâng cấp trải nghiệm tiệc tùng lên tầm cao mới với thiết kế hiện đại và tay cầm gấp gọn tiện lợi. Hệ thống đèn LED dạng vòm cực kỳ bắt mắt có thể tùy chỉnh nhiều chế độ trình diễn ánh sáng sống động qua app JBL PartyBox.

Loa được nâng cấp kết nối không dây Auracast, cho phép bạn ghép nối vô số loa JBL tương thích lại với nhau để tạo nên không gian âm thanh đồng bộ rộng lớn. Pin có thể tháo rời giúp bạn dễ dàng thay pin dự phòng để tiếp tục cuộc vui mà không bị gián đoạn.'),
    -- 36
    (N'Sony SRS-XV900', 17490000, 50, 3, N'Sony', 1,
     N'Bluetooth 5.2, Loa màng X-Balanced Speaker Unit dạng vuông, Âm thanh vòm Omni-directional Party Sound 360 độ, Pin 25 giờ, Cổng Quang Optical kết nối TV (TV Sound Booster), Đèn Ambient Lighting 360 độ, Bánh xe di chuyển.',
     N'Sony SRS-XV900 là chiếc loa tiệc tùng công suất lớn nhất và uy lực nhất của Sony. Thiết kế tháp đứng uy nghi kết hợp hệ thống bánh xe chịu lực và tay cầm phía sau giúp bạn di chuyển khối loa dễ dàng quanh nhà hoặc ra sân vườn.

Sở hữu củ loa woofer X-Balanced màng vuông độc quyền kết hợp hệ thống loa trung và loa cao bố trí xung quanh, SRS-XV900 lan tỏa âm thanh tiệc tùng 360 độ lấp đầy mọi ngóc ngách. Tính năng TV Sound Booster độc đáo biến chiếc loa thành hệ thống trợ lực âm trầm đỉnh cao cho TV nhà bạn khi xem các chương trình ca nhạc hay phim hành động.'),
    -- 37
    (N'Sony ULT TOWER 10', 24990000, 50, 3, N'Sony', 0,
     N'Nút bấm ULT POWER SOUND (ULT1/ULT2), Loa Woofer X-Balanced khổng lồ 320mm x 320mm, Tự động tối ưu hóa âm thanh theo tiếng ồn môi trường, Đèn LED tiệc tùng 360 độ, Tặng kèm Micro không dây cao cấp, Cắm điện 220V.',
     N'Sony ULT TOWER 10 là "gã khổng lồ" âm thanh đứng đầu dòng ULT POWER SOUND, được thiết kế để biến phòng khách nhà bạn thành một câu lạc bộ đêm đúng nghĩa. Mặt trước nổi bật với cụm củ loa Woofer X-Balanced kích thước cực lớn đem đến những nhịp đập rung chuyển sàn nhà.

Hai chế độ ULT1 và ULT2 cung cấp dải âm trầm siêu sâu và bùng nổ chưa từng có. Sản phẩm đi kèm một micro không dây chính hãng chất lượng cao với đầy đủ phím chỉnh Echo/Key control trên thân loa, mang lại trải nghiệm hát karaoke gia đình đỉnh cao.'),
    -- 38
    (N'Harman Kardon Aura Studio 4', 5490000, 50, 3, N'Harman Kardon', 1,
     N'Bluetooth 4.2, Công suất 130W RMS, Subwoofer 132mm hướng xuống sàn + 6 Loa Trung-Cao 40mm bao quanh, Vòm mạ thủy tinh trong suốt với ma trận 324 viên kim cương LED, 5 chủ đề ánh sáng thiên nhiên, Cắm điện 220V.',
     N'Harman Kardon Aura Studio 4 là một tác phẩm nghệ thuật âm thanh kiệt tác. Vòm kính trong suốt biểu tượng chứa đựng bên trong hệ thống 324 viên kim cương bằng nhựa acrylic được chiếu sáng bởi ánh sáng LED huyền ảo, mô phỏng các chủ đề thiên nhiên như cực quang, ngọn lửa hay làn sóng biển.

Về mặt âm thanh, củ loa Subwoofer 132mm đặt ở mặt đáy hướng thẳng xuống sàn tạo ra âm trầm lan tỏa cực kỳ uy lực và sâu lắng. Kết hợp với 6 củ loa trung-cao bố trí vòng tròn, Aura Studio 4 tái hiện không gian âm thanh 360 độ chi tiết và trong trẻo.'),
    -- 39
    (N'Harman Kardon Go + Play 3', 7490000, 50, 3, N'Harman Kardon', 1,
     N'Bluetooth 5.2, Công suất 160W RMS (Hệ thống 3 đường tiếng), Mặt điều khiển bằng kính cảm ứng, Quai xách nhôm đúc nguyên khối, Pin 8 giờ, Tự động cân chỉnh âm thanh theo không gian, Micro kép thu âm xa.',
     N'Harman Kardon Go + Play 3 mang thiết kế bán nguyệt sang trọng lấy cảm hứng từ những chiếc túi xách thời trang cao cấp. Vỏ ngoài bọc vải dệt cao cấp kết hợp mặt trên bằng kính cường lực cảm ứng mịn màng và quai xách nhôm sang trọng.

Cấu trúc âm thanh 3 đường tiếng hoàn toàn mới đem lại độ tách bạch kinh ngạc giữa các dải âm. Củ loa Subwoofer hướng xuống sàn tạo tiếng bass căng trắc đặc trưng, trong khi các củ loa tweeter và midrange tái hiện giọng ca sĩ mượt mà và chân thực đến từng chi tiết.'),
    -- 40
    (N'Harman Kardon Citation Tower', 49990000, 50, 3, N'Harman Kardon', 1,
     N'Hệ thống cặp loa tháp Stereo không dây Hi-End, Công suất 2x200W RMS, Màn hình cảm ứng LCD màu trên đỉnh loa, Kết nối Wi-Fi / Bluetooth / WiSA không độ trễ, Bọc vải len chống bám bẩn Kvadrat Đan Mạch.',
     N'Harman Kardon Citation Tower là cặp loa tháp thông minh hi-end đại diện cho đỉnh cao thiết kế âm thanh nội thất Bắc Âu. Toàn bộ thân loa hình tháp được bọc lớp vải len cao cấp chống bám bẩn và chống cháy do hãng Kvadrat (Đan Mạch) chế tạo thủ công.

Sở hữu tổng công suất lên đến 400W RMS, Citation Tower mang lại sân khấu âm thanh Stereo sống động, chân thực chuẩn hi-fi mà không cần đến bất kỳ sợi dây loa lằng nhằng nào nhờ công nghệ kết nối WiSA không độ trễ. Đỉnh loa tích hợp màn hình cảm ứng LCD màu hiển thị ảnh bìa album nhạc vô cùng tinh tế.'),

    -- ---- SOUNDBAR ----
    -- 41
    (N'Sony HT-A7000', 24990000, 50, 5, N'Sony', 1,
     N'Hệ thống 7.1.2 kênh, Công suất 500W, Dolby Atmos / DTS:X, 2 Loa đánh trần dội trần + 2 Loa Tweeter chùm + Subwoofer tích hợp kép, Công nghệ 360 Spatial Sound Mapping, Cổng HDMI 2.1 (8K/4K 120Hz).',
     N'Sony HT-A7000 là thanh loa Soundbar Flagship cao cấp nhất của Sony, mang lại trải nghiệm xem phim chuẩn rạp chiếu ngay trong căn hộ của bạn. Mặt trên loa phủ kính cường lực đen bóng sang trọng kết hợp lưới kim loại bảo vệ các củ loa bên trong.

Trang bị hai củ loa hắt trần độc đáo cùng công nghệ Vertical Surround Engine và S-Force PRO Front Surround, HT-A7000 tạo ra không gian âm thanh vòm 3D bao phủ khắp căn phòng từ trên xuống dưới và từ hai bên. Chuẩn cổng HDMI 2.1 hỗ trợ đầy đủ các định dạng chơi game PS5 mới nhất như 4K 120Hz và VRR.'),
    -- 42
    (N'Sony HT-S2000', 9990000, 50, 5, N'Sony', 1,
     N'Hệ thống 3.1 kênh, Công suất 250W, Dolby Atmos / DTS:X, Loa Subwoofer kép tích hợp + Loa trung tâm chuyên biệt cho lời thoại, Ứng dụng Sony BRAVIA Connect, Kết nối HDMI eARC.',
     N'Sony HT-S2000 là giải pháp nâng cấp âm thanh TV tối giản nhưng hiệu quả bất ngờ. Thiết kế đơn thanh All-in-one gọn gàng giúp bạn dễ dàng đặt loa ngay bên dưới kệ TV mà không làm che mất mắt hồng ngoại của màn hình.

Mặc dù không có loa sub rời cồng kềnh, HT-S2000 vẫn đem lại âm trầm sâu nhờ cấu trúc subwoofer kép tích hợp bên trong thanh loa. Củ loa trung tâm chuyên biệt giúp làm nổi bật giọng nói nhân vật trong phim hay lời dẫn biên tập viên truyền hình một cách rõ ràng, mạch lạc.'),
    -- 43
    (N'Sony Bravia Theater Bar 9', 29990000, 50, 5, N'Sony', 1,
     N'Hệ thống 13 củ loa cấu trúc 7.0.2 kênh, Dolby Atmos / DTS:X / IMAX Enhanced, Công nghệ 360 Spatial Sound Mapping, Đồng bộ Acoustic Center Sync với TV BRAVIA, Thiết kế mỏng hơn 36% so với A7000.',
     N'Sony Bravia Theater Bar 9 đại diện cho thế hệ loa Soundbar cao cấp mới nhất ra mắt năm 2024. Với kiểu dáng siêu mỏng được bọc vải dệt cao cấp màu xám xước, thanh loa hòa quyện hoàn hảo vào không gian phòng khách hiện đại bên dưới các dòng TV BRAVIA màn hình lớn.

Bên trong thanh loa mỏng gọn là hệ thống 13 củ loa hoạt động đồng bộ, bao gồm loa hắt trần, loa phát ngang và các màng cộng hưởng âm trầm. Khi kết hợp với TV Sony BRAVIA tương thích qua tính năng Acoustic Center Sync, màn hình TV sẽ biến thành củ loa trung tâm, mang lại sự đồng bộ chính xác giữa hình ảnh và âm thanh.'),
    -- 44
    (N'Sony HT-S400', 4990000, 50, 5, N'Sony', 1,
     N'Hệ thống 2.1 kênh kèm Loa Subwoofer không dây củ loa 160mm, Tổng công suất 330W, Màn hình hiển thị OLED nhỏ trên thanh loa, Công nghệ S-Force PRO Front Surround, Kết nối HDMI ARC / Optical / Bluetooth.',
     N'Sony HT-S400 mang đến trải nghiệm giải trí âm thanh rạp chiếu chất lượng cao với mức giá vô cùng hợp lý. Hệ thống gồm một thanh loa chính và một loa siêu trầm không dây mạnh mẽ với màng loa lớn 160mm, giúp việc lắp đặt phòng khách cực kỳ gọn gàng không vướng dây nối.

Thanh loa tích hợp một màn hình OLED nhỏ hiển thị trạng thái kết nối và mức âm lượng tiện lợi. Công nghệ âm thanh vòm giả lập S-Force PRO Front Surround tái tạo không gian âm thanh điện ảnh rộng mở chỉ từ hệ thống loa phía trước.'),
    -- 45
    (N'JBL Bar 1300', 29990000, 50, 5, N'JBL', 1,
     N'Cấu hình đỉnh cao 11.1.4 kênh, Tổng công suất 1170W, 2 Loa vệ tinh không dây chạy pin tháo rời, Loa Subwoofer không dây 10 inch 300W, Dolby Atmos / DTS:X / MultiBeam, AirPlay 2 / Chromecast.',
     N'JBL Bar 1300 là "ông vua" trong thế giới loa Soundbar xem phim gia đình. Điểm độc đáo nhất nằm ở hai loa vệ tinh không dây chạy bằng pin sạc có thể tháo rời khỏi hai đầu thanh loa chính và đặt phía sau lưng người ngồi mà không cần cắm bất kỳ dây nguồn hay dây tín hiệu nào.

Với tổng công suất siêu khủng 1170W cùng củ loa Subwoofer 10 inch đánh rung chuyển căn phòng, Bar 1300 đem lại hiệu ứng âm thanh vòm 3D chuẩn rạp phim Dolby Atmos chân thực tới từng chi tiết, từ tiếng mưa rơi trên mái nhà đến tiếng máy bay nhào lộn trên không.'),
    -- 46
    (N'JBL Bar 1000', 19990000, 50, 5, N'JBL', 1,
     N'Hệ thống 7.1.4 kênh, Công suất 880W, 2 Loa vòm tháo rời pin sạc, Loa Subwoofer không dây 10 inch, Dolby Atmos / DTS:X / MultiBeam, Công nghệ tối ưu giọng nói PureVoice, HDMI eARC Passthrough.',
     N'JBL Bar 1000 thừa hưởng thiết kế loa vệ tinh tháo rời thông minh từ đàn anh Bar 1300 nhưng có mức giá dễ tiếp cận hơn. Hai loa vệ tinh sau khi tháo rời có thể hoạt động độc lập như những chiếc loa Bluetooth di động tiện lợi.

Trang bị 4 củ loa hắt trần kết hợp công nghệ MultiBeam độc quyền, JBL Bar 1000 tái tạo hiệu ứng âm thanh vòm chiều cao rõ nét mà không cần phải khoan tường hay đi dây phức tạp trên trần nhà bạn.'),
    -- 47
    (N'JBL Bar 500', 11990000, 50, 5, N'JBL', 1,
     N'Hệ thống 5.1 kênh, Công suất 590W, Loa Subwoofer không dây 10 inch, Công nghệ 3D MultiBeam & Dolby Atmos Ảo, Thuật toán PureVoice, Wi-Fi tích hợp hỗ trợ AirPlay / Alexa / Chromecast.',
     N'JBL Bar 500 là mẫu soundbar 5.1 kênh tầm trung lý tưởng cho các căn hộ chung cư. Thiết kế thanh loa gọn gàng kết hợp củ loa Subwoofer không dây 10 inch mạnh mẽ giúp không gian giải trí của bạn luôn gọn gàng và hiện đại.

Công nghệ MultiBeam độc quyền chiếu các chùm âm thanh phản xạ qua các bức tường để tạo hiệu ứng âm thanh vòm bao quanh không gian. Công nghệ PureVoice tự động phân tích và làm rõ lời thoại phim, giúp bạn không bỏ lỡ bất kỳ chi tiết kịch tính nào.'),
    -- 48
    (N'JBL Bar 300', 7990000, 50, 5, N'JBL', 1,
     N'Hệ thống 5.0 kênh All-in-one, Công suất 260W, Tích hợp cổng thoát hơi Bass Reflex, Công nghệ Dolby Atmos Ảo & MultiBeam, Tích hợp Wi-Fi truyền nhạc chất lượng cao.',
     N'JBL Bar 300 là giải pháp thanh loa đơn (All-in-one) nhỏ gọn dành cho các phòng ngủ hoặc phòng khách diện tích vừa và nhỏ. Loa không cần cục sub rời nhưng vẫn đem lại dải âm trầm tròn trịa nhờ hệ thống cổng thoát hơi Bass Reflex thông minh ở mặt sau.

Dù có kích thước khiêm tốn, Bar 300 vẫn sở hữu đầy đủ các kết nối thông minh qua Wi-Fi như AirPlay 2, Alexa Multi-Room Music và Chromecast, giúp bạn dễ dàng phát nhạc không dây độ phân giải cao trực tiếp từ điện thoại.'),
    -- 49
    (N'Harman Kardon Citation MultiBeam 1100', 19990000, 50, 5, N'Harman Kardon', 1,
     N'Hệ thống 11 củ loa công suất 630W RMS, Dolby Atmos 3D & MultiBeam, Màn hình cảm ứng LCD đa màu sắc, Tự động cân chỉnh âm thanh (PureVoice / Auto Room Calibration), Vải bọc Kvadrat cao cấp.',
     N'Harman Kardon Citation MultiBeam 1100 kết hợp hoàn hảo giữa thẩm mỹ nội thất cao cấp Đan Mạch và công nghệ âm thanh vòm tiên tiến. Thân loa bọc vải len len Kvadrat tinh tế cùng màn hình cảm ứng LCD sắc nét ở mặt trên làm tăng vẻ sang trọng cho phòng khách.

Sở hữu 11 củ loa bao gồm củ loa đánh trần chuyên biệt và loa phát hướng hai bên, sản phẩm tái tạo không gian âm thanh vòm 3D Dolby Atmos vô cùng sống động mà không cần thêm loa ngoài. Tính năng Auto Room Calibration tự động đo đạc âm thanh phòng để đem lại chất âm tối ưu nhất.'),
    -- 50
    (N'Harman Kardon Citation MultiBeam 700', 11990000, 50, 5, N'Harman Kardon', 1,
     N'Công suất 210W RMS, Công nghệ âm thanh vòm MultiBeam, Màn hình cảm ứng LCD màu, Tích hợp Google Assistant / AirPlay / Chromecast, Bọc vải len chống bám bẩn Kvadrat.',
     N'Harman Kardon Citation MultiBeam 700 sở hữu thiết kế nhỏ gọn hơn đàn anh 1100, hoàn hảo cho TV từ 43 đến 55 inch. Vỏ bọc vải len Kvadrat màu xám hoặc đen mang lại vẻ đẹp tối giản Bắc Âu sang trọng cho ngôi nhà của bạn.

Màn hình cảm ứng LCD hiển thị trực quan thông tin bài hát và giao diện cài đặt. Loa hỗ trợ công nghệ MultiBeam cho hiệu ứng âm thanh vòm rộng mở, đồng thời đóng vai trò như một trung tâm điều khiển nhà thông minh nhờ trợ lý ảo Google Assistant tích hợp.'),

    -- ---- MIC THU AM (KARAOKE / STUDIO) ----
    -- 51
    (N'JBL PartyBox Wireless Mic', 2990000, 50, 7, N'JBL', 1,
     N'Bộ 2 Micro không dây + 1 Bộ thu Receiver cắm cổng 6.3mm, Sóng 2.4GHz độ trễ siêu thấp, Pin micro 20 giờ, Pin bộ thu 12 giờ (Sạc qua USB-C), Củ mic Cardioid tái tạo giọng hát trung thực, Hệ thống chống sốc shock-mount.',
     N'JBL PartyBox Wireless Mic là bộ đôi micro không dây cao cấp thiết kế dành riêng cho các dòng loa JBL PartyBox và các hệ thống loa karaoke gia đình. Micro có thân làm từ chất liệu cao cấp bám tay, tích hợp màn hình LED nhỏ hiển thị dung lượng pin và kênh sóng.

Bộ thu tín hiệu không dây 2.4GHz chuẩn cắm 6.3mm cắm-và-chạy cực kỳ đơn giản, cho khoảng cách kết nối ổn định lên đến 30m mà không bị hiện tượng trễ tiếng hay nhiễu sóng. Màng thu Cardioid giúp tập trung bắt giọng hát ngọt ngào và loại bỏ tối đa tiếng hú rít.'),
    -- 52
    (N'JBL PBM100', 990000, 50, 7, N'JBL', 0,
     N'Micro Dynamic có dây, Mô hình thu Cardioid, Dải tần 50Hz - 15.000Hz, Đầu cắm XLR to 6.3mm dài 3m đi kèm, Lưới bảo vệ bằng thép dây bện có mút lọc gió bên trong, Trọng lượng 110g.',
     N'JBL PBM100 là mẫu micro karaoke có dây truyền thống sở hữu độ bền vượt trội và tính tiện dụng cao. Thân micro bằng kim loại sơn đen nhám chống bám vân tay, cầm đầm tay và chắc chắn.

Được cân chỉnh âm thanh chuyên biệt để ghép nối hoàn hảo với các dòng loa tiệc tùng, PBM100 giúp cất sáng giọng hát của bạn một cách trong trẻo, tự nhiên mà không tốn quá nhiều hơi. Cáp kết nối dài 3m chống đứt gãy cho phép bạn thoải mái di chuyển ca hát trên sân khấu gia đình.'),
    -- 53
    (N'JBL Quantum Stream', 2490000, 50, 6, N'JBL', 0,
     N'2 Củ thu Electret Condenser 14mm, Lấy mẫu chuẩn Studio 24-bit/96kHz, 2 Mô hình thu (Cardioid & Omnidirectional), Nút chạm Mute nhanh trên đỉnh mic có đèn chỉ báo, Chân đế nhôm xoay 360 độ, Vòng đèn RGB.',
     N'JBL Quantum Stream là chiếc micro thu âm USB để bàn chuyên nghiệp dành cho Streamer, Gamer và Podcaster. Thiết kế vỏ nhôm cao cấp kết hợp chân đế kim loại xoay linh hoạt và vòng đèn LED RGB tạo điểm nhấn ấn tượng cho góc máy tính của bạn.

Sở hữu 2 củ thu Condenser chuẩn 24-bit/96kHz, micro cho phép chuyển đổi dễ dàng giữa chế độ thu đơn Cardioid (tập trung giọng nói cá nhân) và chế độ thu đa hướng Omnidirectional (phù hợp cho các buổi phỏng vấn nhiều người xung quanh bàn).'),
    -- 54
    (N'JBL Quantum Stream Studio', 3490000, 50, 6, N'JBL', 0,
     N'3 Củ thu Condenser 14mm, Lấy mẫu 24-bit/192kHz, 4 Mô hình thu âm (Cardioid, Stereo, Bi-directional, Omnidirectional), Công nghệ nén dải động DRC chống giật tiếng, Đèn hiển thị mức âm lượng VU meter.',
     N'JBL Quantum Stream Studio là phiên bản micro thu âm để bàn chuẩn Flagship cao cấp nhất thuộc dòng Quantum. Mặt trước micro trang bị núm xoay đa năng kết hợp hệ thống đèn đo âm lượng VU meter trực quan, giúp bạn kiểm soát mức tín hiệu thu vào một cách chính xác nhất.

Hệ thống 3 củ thu Condenser cung cấp 4 mô hình thu âm linh hoạt, đáp ứng mọi kịch bản thu âm từ thu nhạc cụ, hát hò, phát sóng trực tiếp cho đến thu âm Podcast 2 người đối diện. Công nghệ nén DRC tự động ngăn chặn tình trạng vỡ tiếng khi bạn cất giọng quá to đột ngột.'),
    -- 55
    (N'JBL Quantum Stream Talk', 1490000, 50, 6, N'JBL', 0,
     N'1 Củ thu Electret Condenser 14mm, Mô hình thu Super Cardioid siêu định hướng, Chất lượng 24-bit/96kHz, Chân đế giảm chấn chống rung lắc bàn, Nút xoay chỉnh gain tích hợp mute, Cắm USB Type-C.',
     N'JBL Quantum Stream Talk được thiết kế chuyên biệt để nâng cao chất lượng giọng nói trong các cuộc họp Zoom, học trực tuyến hay đàm thoại game gia đình. Thiết kế chân đế hợp kim nhỏ gọn giúp tiết kiệm tối đa diện tích bàn làm việc.

Mô hình thu Super Cardioid góc hẹp giúp micro chỉ tập trung bắt trọn giọng nói của người ngồi trực diện, đồng thời triệt tiêu hiệu quả các tạp âm xung quanh như tiếng gõ bàn phím cơ hay tiếng quạt máy lạnh.'),
    -- 56
    (N'Sony C-80', 11990000, 50, 6, N'Sony', 0,
     N'Micro Condenser màng hoành lớn (Large Diaphragm), Định hướng Uni-directional (Cardioid), Cấu trúc củ thu capsule kép từ C-800G huyền thoại, Cổng XLR 3-pin, Công tắc cắt tần số thấp Low-cut (-10dB Pad).',
     N'Sony C-80 là micro thu âm Studio chuyên nghiệp thừa hưởng cấu trúc màng thu kép từ huyền thoại Sony C-800G có giá hàng chục ngàn đô la. Thân micro bọc kim loại kiên cố giúp triệt tiêu hiện tượng cộng hưởng âm thanh không mong muốn.

Sản phẩm giúp tái tạo dải trung ấm áp, giàu cảm xúc và dải cao mịn màng, làm nổi bật giọng hát của ca sĩ hoặc âm sắc của guitar acoustic. Thiết kế chống hiệu ứng gần (Proximity Effect) giúp khoảng cách thu âm linh hoạt mà không bị biến dạng dải trầm.'),
    -- 57
    (N'Sony C-100', 29990000, 50, 6, N'Sony', 0,
     N'Micro Condenser 2 chiều chuẩn Hi-Res Audio, Đáp ứng tần số 20Hz đến 50.000Hz, 3 Mô hình thu (Uni/Omni/Bi-directional), Củ thu kép (1 củ thu dải rộng + 1 củ thu dải siêu cao), Cổng kết nối XLR mạ vàng.',
     N'Sony C-100 là micro phòng thu Hi-Res Audio đạt chuẩn mực đỉnh cao thế giới. Cấu trúc hai củ thu độc lập bên trong gồm một củ thu dải tần rộng và một củ thu dải siêu cao cho phép ghi lại tần số âm thanh kéo dài tới 50kHz.

Mọi sắc thái mỏng manh nhất của nhạc cụ cổ điển hay hơi thở tinh tế của giọng hát đều được Sony C-100 ghi lại trọn vẹn và sống động. Mẫu mic này là sự lựa chọn số một cho các phòng thu âm thương mại chuyên nghiệp.'),
    -- 58
    (N'Sony ECM-S1', 9990000, 50, 6, N'Sony', 1,
     N'Hệ thống Micro không dây Condenser + Bộ thu Receiver kết nối trực tiếp Chân MI Shoe / USB-C / 3.5mm, 3 Củ thu 14mm, Bluetooth 5.3 LC3plus, Pin 3 giờ, Mút lọc gió Pop Filter đi kèm, Chuẩn thu âm Kỹ thuật số 24-bit.',
     N'Sony ECM-S1 là giải pháp micro thu âm không dây để bàn mang tính đột phá cho các nhà sáng tạo nội dung, Vlogger và Podcaster. Hệ thống gồm một micro phát không dây và một bộ thu Receiver nhỏ gọn kết nối trực tiếp với máy ảnh Sony qua chân ngàm MI Shoe hoặc cắm qua máy tính bằng dây USB-C.

Với 3 củ thu Condenser 14mm chất lượng cao và chuẩn truyền tải Bluetooth LC3plus độ trễ thấp, ECM-S1 mang lại chất lượng thu âm trong trẻo chuẩn studio ngay cả khi micro đặt cách xa máy quay tới 150m.'),
    -- 59
    (N'Sony F-V120', 490000, 50, 7, N'Sony', 0,
     N'Micro Dynamic có dây, Đáp ứng tần số 60Hz - 12.000Hz, Dây cáp liền dài 3m trang bị đầu cắm Mini Plug 3.5mm kèm Adapter 6.3mm, Công tắc Bật/Tắt (On/Off) trên thân mic, Trọng lượng nhẹ 137g.',
     N'Sony F-V120 là mẫu micro cầm tay có dây phổ thông phục vụ nhu cầu ca hát giải trí gia đình, phát biểu hội nghị hay giảng dạy. Thân micro bằng nhựa cao cấp nhẹ nhàng, cầm không mỏi tay.

Công tắc gạt On/Off tiện lợi ngay trên thân mic giúp người dùng chủ động ngắt tiếng khi không sử dụng. Dây cáp dài 3m tích hợp sẵn giắc cắm linh hoạt tương thích với hầu hết các dàn âm thanh, ampli karaoke hay loa kéo hiện nay.'),
    -- 60
    (N'Sony F-V420', 990000, 50, 7, N'Sony', 0,
     N'Micro Dynamic có dây, Nam châm Neodymium công suất cao, Tần số 80Hz - 15.000Hz, Thân kim loại kẽm nguyên khối đúc áp lực, Cáp đồng OFC dài 3m chân cắm mạ vàng, Công tắc On/Off chống ồn tiếng lách cách.',
     N'Sony F-V420 là dòng micro có dây chất lượng cao dành cho các tín đồ đam mê hát karaoke tại gia. Thân micro được làm từ đúc kim loại kẽm đầm tay, bền bỉ và đem lại cảm giác cầm nắm đắt tiền.

Trang bị nam châm Neodymium mật độ từ tính cao, F-V420 giúp tái tạo giọng hát uy lực, dày dặn và chống dội âm cực tốt. Công tắc nguồn thiết kế đặc biệt không gây ra tiếng "bốp" khó chịu ra loa mỗi khi bật hay tắt micro.')
) AS v(ProductName, Price, StockQuantity, CategoryID, BrandName, IsWireless, Specifications, Description)
JOIN Brands b ON b.BrandName = v.BrandName
WHERE NOT EXISTS (SELECT 1 FROM Products p WHERE p.ProductName = v.ProductName);

PRINT N'Da them san pham: ' + CAST(@@ROWCOUNT AS NVARCHAR(10));

-- =====================================================================
-- PHAN 2: LINK ANH + VIDEO
-- Dien link da kiem tra (mo duoc tren trinh duyet) vao bang tam duoi day.
-- San pham nao chua du ca 3 anh va video se duoc giu nguyen (NULL), khong bi ghi de.
-- Img1..Img3 : link anh truc tiep (.jpg/.png/.webp)
-- VideoUrl   : link video (VD: https://www.youtube.com/embed/<VIDEO_ID>)
-- =====================================================================
DECLARE @Sep NVARCHAR(5) = N'|';   -- dau ngan cach 3 anh trong cot ImageUrl

CREATE TABLE #Media (
    ProductName NVARCHAR(255) PRIMARY KEY,
    Img1 NVARCHAR(MAX) NULL,
    Img2 NVARCHAR(MAX) NULL,
    Img3 NVARCHAR(MAX) NULL,
    VideoUrl NVARCHAR(MAX) NULL
);

INSERT INTO #Media (ProductName, Img1, Img2, Img3, VideoUrl) VALUES
(N'Marshall Emberton II', NULL, NULL, NULL, NULL),
(N'Marshall Willen', NULL, NULL, NULL, NULL),
(N'JBL Charge 5', NULL, NULL, NULL, NULL),
(N'JBL Flip 6', NULL, NULL, NULL, NULL),
(N'JBL Xtreme 4', NULL, NULL, NULL, NULL),
(N'Sony ULT FIELD 1', NULL, NULL, NULL, NULL),
(N'Sony SRS-XB100', NULL, NULL, NULL, NULL),
(N'Harman Kardon Onyx Studio 8', NULL, NULL, NULL, NULL),
(N'Marshall Woburn III', NULL, NULL, NULL, NULL),
(N'Marshall Stanmore III', NULL, NULL, NULL, NULL),
(N'JBL PartyBox 710', NULL, NULL, NULL, NULL),
(N'JBL PartyBox 310', NULL, NULL, NULL, NULL),
(N'JBL PartyBox Club 120', NULL, NULL, NULL, NULL),
(N'Sony SRS-XV900', NULL, NULL, NULL, NULL),
(N'Sony ULT TOWER 10', NULL, NULL, NULL, NULL),
(N'Harman Kardon Aura Studio 4', NULL, NULL, NULL, NULL),
(N'Harman Kardon Go + Play 3', NULL, NULL, NULL, NULL),
(N'Harman Kardon Citation Tower', NULL, NULL, NULL, NULL),
(N'Sony HT-A7000', NULL, NULL, NULL, NULL),
(N'Sony HT-S2000', NULL, NULL, NULL, NULL),
(N'Sony Bravia Theater Bar 9', NULL, NULL, NULL, NULL),
(N'Sony HT-S400', NULL, NULL, NULL, NULL),
(N'JBL Bar 1300', NULL, NULL, NULL, NULL),
(N'JBL Bar 1000', NULL, NULL, NULL, NULL),
(N'JBL Bar 500', NULL, NULL, NULL, NULL),
(N'JBL Bar 300', NULL, NULL, NULL, NULL),
(N'Harman Kardon Citation MultiBeam 1100', NULL, NULL, NULL, NULL),
(N'Harman Kardon Citation MultiBeam 700', NULL, NULL, NULL, NULL),
(N'JBL PartyBox Wireless Mic', NULL, NULL, NULL, NULL),
(N'JBL PBM100', NULL, NULL, NULL, NULL),
(N'JBL Quantum Stream', NULL, NULL, NULL, NULL),
(N'JBL Quantum Stream Studio', NULL, NULL, NULL, NULL),
(N'JBL Quantum Stream Talk', NULL, NULL, NULL, NULL),
(N'Sony C-80', NULL, NULL, NULL, NULL),
(N'Sony C-100', NULL, NULL, NULL, NULL),
(N'Sony ECM-S1', NULL, NULL, NULL, NULL),
(N'Sony F-V120', NULL, NULL, NULL, NULL),
(N'Sony F-V420', NULL, NULL, NULL, NULL);

UPDATE p
SET p.ImageUrl = m.Img1 + @Sep + m.Img2 + @Sep + m.Img3,
    p.VideoUrl = m.VideoUrl
FROM Products p
JOIN #Media m ON m.ProductName = p.ProductName
WHERE m.Img1 IS NOT NULL AND m.Img2 IS NOT NULL AND m.Img3 IS NOT NULL AND m.VideoUrl IS NOT NULL;

PRINT N'Da cap nhat media: ' + CAST(@@ROWCOUNT AS NVARCHAR(10));

DROP TABLE #Media;
GO

-- Kiem tra nhanh
SELECT c.CategoryName, COUNT(*) AS SoSanPham
FROM Products p JOIN Categories c ON c.CategoryID = p.CategoryID
GROUP BY c.CategoryID, c.CategoryName ORDER BY c.CategoryID;
GO
