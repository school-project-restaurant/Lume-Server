use chrono::{Duration, Utc};
use rand::prelude::SliceRandom;
use rand::Rng;
use serde::{Deserialize, Serialize};
use serde_json::json;
use sha2::{Digest, Sha256};
use std::fs;
use uuid::Uuid;

#[derive(Serialize, Deserialize)]
struct Customer {
    id: String,
    name: String,
    surname: String,
    email: String,
    phoneNumber: String,
    reservationsId: Vec<String>,
}

#[derive(Serialize, Deserialize)]
struct Staff {
    id: String,
    name: String,
    surname: String,
    phoneNumber: String,
    passwordHash: String,
    salary: i32,
    isActive: bool,
    monthHours: i32,
}

#[derive(Serialize, Deserialize)]
struct Table {
    number: i32,
    reservationsId: Vec<String>,
    seats: i32,
}

#[derive(Serialize, Deserialize)]
struct Reservation {
    id: String,
    customerId: String,
    date: String,
    tableNumber: i32,
    guestCount: i32,
    status: String,
    notes: String,
}

fn randomNotes() -> String {
    let mut rng = rand::thread_rng();
    // Vector of subject phrases
    let subjects = vec![
        "I want",
        "She likes",
        "We enjoy",
        "He craves",
        "They love",
        "I would like",
    ];

    // Vector of food items
    let foods = vec![
        "pizza",
        "pasta",
        "burger",
        "salad",
        "sushi",
        "steak",
        "ramen",
        "tacos",
        "sandwich",
        "ice cream",
    ];

    // Choose a random subject and random food item from the vectors
    let subject = subjects.choose(&mut rng).expect("Subjects vector is empty");
    let food = foods.choose(&mut rng).expect("Food vector is empty");

    // Generate and print the random note
    if rng.gen() {
        format!("{} {}.", subject, food)
    }
    else {
        "".to_string()
    }
}

fn randomDate() -> String {
    // Get today's date (without time)
    let today = Utc::now().date_naive();
    // Convert into a NaiveDateTime set to midnight (00:00:00)
    let today_midnight = today
        .and_hms_opt(0, 0, 0)
        .expect("Invalid time components for midnight");

    // Generate a random number of days to add (0 to 730 days)
    let mut rng = rand::thread_rng();
    let random_days = rng.gen_range(0..=730); // random_days is an i32

    // Add the random_days as a Duration (cast i32 to i64)
    let random_date = today_midnight + Duration::days(random_days as i64);

    // Format the datetime in the desired format
    random_date.format("%Y-%m-%dT%H:%M:%S").to_string()
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
    for _ in 0..10 {
        let customer_name = generate_name();
        let customer_surname = generate_name();
        let customer_email = format!("{}{}@example.com", customer_name, customer_surname);
        let customer_phone = generate_phone_number();

        let customer = Customer {
            id: Uuid::new_v4().to_string(),
            name: customer_name,
            surname: customer_surname,
            email: customer_email,
            phoneNumber: customer_phone,
            reservationsId: Vec::new(),
        };

        customers.push(customer);
    }

    // Generate 20 staff members
    for _ in 0..10 {
        let staff_name = generate_name();
        let staff_surname = generate_name();
        let staff_phone = generate_phone_number();

        let staff = Staff {
            id: Uuid::new_v4().to_string(),
            name: staff_name,
            surname: staff_surname,
            phoneNumber: staff_phone,
            passwordHash: generate_password_hash(&random_string()),
            salary: (1000 + 10 * rng.gen_range(1..70)),
            isActive: rng.gen(),
            monthHours: (100 + 10 * rng.gen_range(1..7)),
        };

        staff_members.push(staff);
    }

    // Generate 10 tables
    for i in 1..=15 {
        let table = Table {
            number: i,
            reservationsId: Vec::new(),
            seats: rng.gen_range(2..=10), // Random number of seats between 2 and 10
        };

        tables.push(table);
    }

    // Generate 100 reservations
    for _ in 0..30 {
        // Randomly pick a customer's index and a table index
        let customer_index = rng.gen_range(0..customers.len());
        let table_index = rng.gen_range(0..tables.len());

        let possibleStatus: Vec<String> = vec![
            "confirmed".to_string(),
            "pending".to_string(),
            "rejected".to_string(),
        ];
        let reservation_id = Uuid::new_v4().to_string();
        let reservation = Reservation {
            id: reservation_id.clone(),
            customerId: customers[customer_index].id.clone(),
            date: randomDate(),
            tableNumber: tables[table_index].number,
            guestCount: tables[table_index].seats - rng.gen_range(0..tables[table_index].seats),
            status: possibleStatus[rng.gen_range(0..3)].clone(),
            notes: randomNotes(),
        };

        // Add the reservation id to the customer's reservations_id list
        customers[customer_index]
            .reservationsId
            .push(reservation_id.clone());
        // Add the reservation id to the table's reservations_id list
        tables[table_index].reservationsId.push(reservation_id);

        reservations.push(reservation);
    }

    let json_output = json!({
        "customers": customers,
        "staff": staff_members,
        "tables": tables,
        "reservations": reservations,
    });

    let _ = fs::write(
        "seeders.json",
        serde_json::to_string_pretty(&json_output).unwrap(),
    );
    // println!("{}", serde_json::to_string_pretty(&json_output).unwrap());
}
