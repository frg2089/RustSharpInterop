#[repr(C)]
pub struct Res {
    data: *mut u8,
    size: usize,
}
impl Res {
    fn get_data(&self) -> Vec<u8> {
        unsafe {
            return Vec::from_raw_parts(self.data, self.size, self.size);
        }
    }
}

#[no_mangle]
pub extern "C" fn test(op: extern "C" fn(str: *const u8, size: usize) -> Res) {
    println!("Hello Rust DLL!");

    let str = "Call from Rust!";
    let res = op(str.as_ptr(), str.len());

    println!("{}", res.size);
    println!("{:?}", res.get_data());
}
