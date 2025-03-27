use rand::Rng;
use serde::{Deserialize, Serialize};
use serde_json::json;
use sha2::{Digest, Sha256};
use uuid::Uuid;

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

    for _ in 0..5 {
        // Shorter names for simplicity
        name.push(
            consonants
                .chars()
                .nth(rng.gen_range(0..consonants.len()))
                .unwrap(),
        );
        name.push(vowels.chars().nth(rng.gen_range(0..vowels.len())).unwrap());
    }
    name
}

fn random_string() -> String {
    let characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    let mut rng = rand::thread_rng();

    (0..16)
        .map(|_| {
            let idx = rng.gen_range(0..characters.len());
            characters.chars().nth(idx).unwrap()
        })
        .collect()
}

fn generate_phone_number() -> String {
    let mut rng = rand::thread_rng();
    format!(
        "{}-{}-{}",
        rng.gen_range(100..999),
        rng.gen_range(100..999),
        rng.gen_range(1000..9999)
    )
}

fn generate_password_hash(password: &str) -> String {
    let mut hasher = Sha256::new();
    hasher.update(password);
    format!("{:x}", hasher.finalize())
}

fn main() {
    let mut customers = Vec::new();
    let mut staff_members = Vec::new();
    let mut tables = Vec::new();
    let mut reservations = Vec::new();
    let mut rng = rand::thread_rng();

    // Generate 50 customers
    for _ in 0..50 {
        let customer_name = generate_name();
        let customer_surname = generate_name();
        let customer_email = format!("{}{}@example.com", customer_name, customer_surname);
        let customer_phone = generate_phone_number();

        let customer = Customer {
            id: Uuid::new_v4().to_string(),
            name: customer_name,
            surname: customer_surname,
            email: customer_email,
            phone: customer_phone,
            reservations_id: Vec::new(),
        };

        customers.push(customer);
    }

    // Generate 20 staff members
    for _ in 0..20 {
        let staff_name = generate_name();
        let staff_surname = generate_name();
        let staff_phone = generate_phone_number();

        let staff = Staff {
            id: Uuid::new_v4().to_string(),
            name: staff_name,
            surname: staff_surname,
            phone: staff_phone,
            password_hash: generate_password_hash(&random_string()),
            salary: 50000,
            is_active: true,
            month_hours: 160,
        };

        staff_members.push(staff);
    }

    // Generate 10 tables
    for i in 1..=10 {
        let table = Table {
            number: i,
            reservations_id: Vec::new(),
            seats: rng.gen_range(2..=10), // Random number of seats between 2 and 10
        };

        tables.push(table);
    }

    // Generate 100 reservations
    for _ in 0..100 {
        // Randomly pick a customer's index and a table index
        let customer_index = rng.gen_range(0..customers.len());
        let table_index = rng.gen_range(0..tables.len());

        let reservation_id = Uuid::new_v4().to_string();
        let reservation = Reservation {
            id: reservation_id.clone(),
            customer_id: vec![customers[customer_index].id.clone()],
            date: "2023-10-01T19:00:00".to_string(),
            table: vec![tables[table_index].number],
            guest_count: 2,
            status: "confirmed".to_string(),
            notes: "No special requests".to_string(),
        };

        // Add the reservation id to the customer's reservations_id list
        customers[customer_index].reservations_id.push(reservation_id.clone());
        // Add the reservation id to the table's reservations_id list
        tables[table_index].reservations_id.push(reservation_id);

        reservations.push(reservation);
    }

    let json_output = json!({
        "customers": customers,
        "staff": staff_members,
        "tables": tables,
        "reservations": reservations,
    });

    println!("{}", serde_json::to_string_pretty(&json_output).unwrap());
}
