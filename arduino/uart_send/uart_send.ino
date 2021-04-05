float buffer[2] = {0.9281324, 0.4593406};//{0.5, 123.456};
uint32_t header = 0xAABBCCDD;
uint32_t checksum = 0xA0B0CDEF;
static char *py;
void setup() {
  Serial.begin(115200);

//  Serial.print("float=");
//  Serial.println(sizeof(float)); // output: "float=4"
//  Serial.print("data_y=");
//  Serial.println(sizeof(data_y)); // output: "data_y=4"
//  Serial.print("buffer=");
//  Serial.println(sizeof(buffer)); // output: "buffer=8"
}

void loop() {
  Serial.write((char*)&header, sizeof(header) );
  py = (char*)buffer;
  for(int i = 0; i<1000; i++){
    Serial.write(py, sizeof(buffer) );
//    Serial.print(buffer[0]);Serial.print(", ");Serial.println(buffer[1]);
    buffer[0] = buffer[0] + 1.0;
    delay(30);
  }

  Serial.write((char*)&checksum, sizeof(checksum) );
  delay(30);
  while(1);
}
