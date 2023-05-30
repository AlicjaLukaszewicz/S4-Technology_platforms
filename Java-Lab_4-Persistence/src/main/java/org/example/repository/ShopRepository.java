package org.example.repository;

import org.example.entity.Shop;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;

public class ShopRepository extends JpaRepository<Shop, String> {

    /**
     * @param emf thread safe factory which will be used for creating {@link EntityManager}
     */
    public ShopRepository(EntityManagerFactory emf) {
        super(emf, Shop.class);
    }

    public Shop findByName(String name) {
        EntityManager em = getEmf().createEntityManager();
        try {
            return em.createQuery("SELECT shop FROM Shop shop WHERE shop.name = :name", Shop.class).setParameter("name", name).getSingleResult();
        } finally {
            em.close();
        }
    }

}