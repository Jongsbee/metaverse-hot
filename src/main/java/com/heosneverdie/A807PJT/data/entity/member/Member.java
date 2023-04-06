package com.heosneverdie.A807PJT.data.entity.member;

import com.heosneverdie.A807PJT.common.BaseEntity;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import java.util.List;

@Table(name = "Member")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Member extends BaseEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    @Column(nullable = false)
    private String nickname;
    @Column(nullable = false, name = "firebase_id")
    private String firebaseId;
    @OneToOne(mappedBy = "member")
    private Classes classes;
    @OneToOne(mappedBy = "member")
    private Account account;

    @OneToOne(mappedBy = "member")
    private Coupon coupon;
    
    public void updateClasses(Classes classes) {
        this.classes = classes;
    }
    public void updateAccount(Account account) {
        this.account = account;
    }

    public void updateCoupon(Coupon coupon) {
        this.coupon = coupon;
    }



}
