use serde::{Serialize, Deserialize};
use serde_json::json;
use uuid::Uuid;
use rand::Rng;
use sha2::{Sha256, Digest};

#[derive(Serialize, Deserialize)]
struct Customer {
    id: String,
    name: String,
    surname: String,
    email: String,
    phone: String,
    reservations_id: Vec<String>,
}

#[derive(Serialize, Deserialize)]
struct Staff {
    id: String,
    name: String,
    surname: String,
    phone: String,
    password_hash: String,
    salary: i32,
    is_active: bool,
    month_hours: i32,
}

#[derive(Serialize, Deserialize)]
struct Table {
    number: i32,
    reservations_id: Vec<String>,
    seats: i32,
}

#[derive(Serialize, Deserialize)]
struct Reservation {
    id: String,
    customer_id: Vec<String>,
    date: String,
    table: Vec<i32>,
    guest_count: i32,
    status: String,
    notes: String,
}

fn generate_name() -> String {
    let mut name = String::new();
    let vowels = "aeiou";
    let consonants = "bcdfghjklmnpqrstvwxyz";
    let mut rng = rand::thread_rng();

    for _ in 0..10 {
        name.push(consonants.chars().nth(rng.gen_range(0..consonants.len())).unwrap());
        name.push(vowels.chars().nth(rng.gen_range(0..vowels.len())).unwrap());
    }
    name
}

fn generate_phone_number() -> String {
    let mut rng = rand::thread_rng();
    format!("{}-{}-{}", 
        rng.gen_range(100..999), 
        rng.gen_range(100..999), 
        rng.gen_range(1000..9999))
}

fn generate_password_hash(password: &str) -> String {
    let mut hasher = Sha256::new();
    hasher.update(password);
    format!("{:x}", hasher.finalize())
}

fn main() {
    let customer_name = generate_name();
    let customer_surname = customer_name.clone();
    let customer_email = format!("{}{}@example.com", customer_name, customer_surname);
    let customer_phone = generate_phone_number();

    let customer = Customer {
        id: Uuid::new_v4().to_string()  ,
        name: customer_name,
        surname: customer_surname,
        email: customer_email,
        phone: customer_phone,
        reservations_id: Vec::new(),
    };

    let staff = Staff {
        id: Uuid::new_v4().to_string(),
        name: customer.name.clone(),
        surname: customer.surname.clone(),
        phone: customer.phone.clone(),
        password_hash: generate_password_hash("password123"),
        salary: 50000,
        is_active: true,
        month_hours: 160,
    };

    let table = Table {
        number: 1,
        reservations_id: Vec::new(),
        seats: 4,
    };

    let reservation = Reservation {
        id: Uuid::new_v4().to_string(),
        customer_id: vec![customer.id],
        date: "2023-10-01T19:00:00".to_string(),
        table: vec![table.number],
        guest_count: 2,
        status: "confirmed".to_string(),
        notes: "No special requests".to_string(),
    };

    let json_output = json!({
        "customers": customer,
        "staff": staff,
        "tables": table,
        "reservations": reservation,
    });

    println!("{}", serde_json::to_string_pretty(&json_output).unwrap());
}
