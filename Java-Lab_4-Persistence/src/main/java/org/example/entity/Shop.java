package org.example.entity;

import lombok.*;

import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import java.util.ArrayList;
import java.util.List;

@EqualsAndHashCode
@Entity
@Getter
@Setter
public class Shop {
    @Id
    private String name;

    private long value;

    @OneToMany(mappedBy = "shop", fetch = FetchType.EAGER)
    private List<Tea> teas = new ArrayList<>();

    public Shop() {
    }

    public Shop(String name, long value) {
        this.name = name;
        this.value = value;
    }


    @Override
    public String toString() {
        return "Shop {" +
                "name: '" + name + '\'' +
                ", value: " + value +
                '}';
    }
}