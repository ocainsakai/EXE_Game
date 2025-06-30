Pull về trước khi code
  git pull origin main

Code xong thì commit → push
  git add .
  git commit -m "Mô tả thay đổi"
  git push origin main

Tránh cùng sửa 1 file cùng lúc, đặc biệt các file sau:
  *.unity (scenes)
  *.prefab
  ProjectSettings/

Giải quyết conflict
  Nếu bị conflict → Git báo lỗi khi git pull → chỉnh sửa → commit lại.

Khi Code gặp vấn đề, cần báo ngay lên group Zalo
