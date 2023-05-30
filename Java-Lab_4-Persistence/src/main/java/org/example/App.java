package org.example;

import org.example.entity.Shop;
import org.example.entity.Tea;
import org.example.repository.TeaRepository;
import org.example.repository.ShopRepository;

import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;
import java.util.Scanner;

public class App {
    public static void main(String[] args) {
        EntityManagerFactory emf = Persistence.createEntityManagerFactory("lab4Pu");

        TeaRepository teaRepository = new TeaRepository(emf);
        ShopRepository shopRepository = new ShopRepository(emf);

        Shop five_clock = new Shop("Five O'clock", 40);
        Shop tea_janowicz = new Shop("Tea Janowicz", 99);
        shopRepository.add(five_clock);
        shopRepository.add(tea_janowicz);

        Tea ceylon_lady_gray = new Tea("Ceylon Lady Gray", 29, five_clock);
        Tea sycyliana = new Tea("Sycyliana", 27, five_clock);
        Tea bancha_mango = new Tea("Bancha Mango", 14, tea_janowicz);

        teaRepository.add(ceylon_lady_gray);
        teaRepository.add(sycyliana);
        teaRepository.add(bancha_mango);

        boolean run = true;
        Scanner scanner = new Scanner(System.in);

        while (run) {
            showMenu();
            int option = scanner.nextInt();
            switch (option) {
                case 1:
                    System.out.println("Tea name, price, shop:");
                    String name = scanner.next();
                    long price = scanner.nextLong();
                    String shopName = scanner.nextLine();
                    Shop shop = shopRepository.findByName(shopName);
                    Tea newTea = new Tea(name, price, shop);
                    teaRepository.add(newTea);
                    break;
                case 2:
                    System.out.println("Shop name and value:");
                    name = scanner.next();
                    long value = scanner.nextLong();
                    Shop newShop = new Shop(name, value);
                    shopRepository.add(newShop);
                    break;
                case 3:
                    System.out.println("Teas with price lower than:");
                    price = scanner.nextLong();
                    for (Tea m: teaRepository.findAllWithPriceLowerThan(price)) {
                        System.out.println(m.toString());
                    }
                    break;
                case 4:
                    System.out.println("From Shop cheaper than");
                    name = scanner.next();
                    price = scanner.nextLong();
                    for (Tea t: teaRepository.findAllFromShopCheaperThan(name, price)) {
                        System.out.println(t.toString());
                    }
                    break;
                case 5:
                    for (Tea t: teaRepository.findAll()) {
                        System.out.println(t.toString());
                    }
                    break;
                case 6:
                    for (Shop t: shopRepository.findAll()) {
                        System.out.println(t.toString());
                    }
                    break;
                case 7:
                    System.out.println("Tea name");
                    name = scanner.next();
                    Tea tea = teaRepository.find(name);
                    teaRepository.delete(tea);
                    break;
                case 8:
                    System.out.println("Shop name");
                    name = scanner.next();
                    shop = shopRepository.find(name);
                    for (Tea b: shop.getTeas()){
                        //b.toString();
                        teaRepository.delete(b);
                    }
                    shop.setTeas(null);
                    shopRepository.delete(shop);
                    break;
                case 9:
                    run = false;
                    break;
                default:
                    System.out.println("Wrong value!");
            }
        }


        emf.close();
    }
    public static void showMenu() {
        System.out.println("Menu:");
        System.out.println("1: Add tea.");
        System.out.println("2: Add shop.");
        System.out.println("3: Show teas with price < ");
        System.out.println("4: Show teas from with price < ");
        System.out.println("5: Show all teas. ");
        System.out.println("6: Show all shops. ");
        System.out.println("7: Delete tea. ");
        System.out.println("8: Delete shop. ");
        System.out.println("9: Exit :(");
    }
}