package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.springframework.boot.context.properties.bind.DefaultValue;

import javax.persistence.*;

@Table(name = "COUPON")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Coupon {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    // member와 1:1 매핑
    @OneToOne
    @JoinColumn(nullable = false, name = "member_id")
    private Member member;
    @Column(nullable = false, name = "coupon_1")
    private Boolean isCoupon1;
    @Column(nullable = false, name = "coupon_2")
    private Boolean isCoupon2;

    @Column(nullable = false, name = "coupon_3")
    private Boolean isCoupon3;

    @Column(nullable = false, name = "coupon_4")
    private Boolean isCoupon4;

    @Column(nullable = false, name = "coupon_5")
    private Boolean isCoupon5;

    public void updateCoupon1() {
        this.isCoupon1 = true;
    }
    public void updateCoupon2() {
        this.isCoupon2 = true;
    }
    public void updateCoupon3() {
        this.isCoupon3 = true;
    }
}
