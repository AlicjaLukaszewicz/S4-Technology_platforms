package org.example.entity;

import lombok.*;

import javax.persistence.*;

@EqualsAndHashCode
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Tea {
    @Id
    private String name;

    @Setter
    private double price;

    @Setter
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "shop")
    private Shop shop;

    @Override
    public String toString() {
        return Name: '" + name + '\'' + ", price: " + price + ", shop: " + shop;
    }
}